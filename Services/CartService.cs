using AutoMapper;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Models.DTO.Response;
using kit_stem_api.Repositories;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace kit_stem_api.Services
{
    public class CartService : ICartService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UnitOfWork _unitOfWork;

        public CartService(IMapper mapper, UserManager<ApplicationUser> userManager, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse> CreateAsync(string userName, CartDTO cartDTO)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status401Unauthorized)
                        .AddDetail("message", "Thêm vào giỏ hàng thất bại!")
                        .AddError("notFound", "Không tìm thấy tài khoản ngay lúc này!");
                }

                var existingCart = await _unitOfWork.CartRepository.GetFilterAsync(
                    c => c.UserId == user.Id && c.PackageId == cartDTO.PackageId);

                if (existingCart.Item1.Any())
                {
                    var cart = existingCart.Item1.First();
                    cart.PackageQuantity += cartDTO.PackageQuantity;

                    await _unitOfWork.CartRepository.UpdateAsync(cart);
                    return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Cập nhật số lượng gói kit trong giỏ hàng thành công!");
                }
                else
                {
                    var cart = new Cart
                    {
                        UserId = user.Id,
                        PackageId = cartDTO.PackageId,
                        PackageQuantity = cartDTO.PackageQuantity,
                    };

                    var package = await _unitOfWork.PackageRepository.GetByIdAsync(cart.PackageId);
                    if (package == null)
                    {
                        return new ServiceResponse()
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status404NotFound)
                            .AddDetail("message", "Thêm gói kit vào giỏ hàng thất bại!")
                            .AddError("notFound", "Không tìm thấy gói kit ngay lúc này!");
                    }

                    await _unitOfWork.CartRepository.CreateAsync(cart);
                    return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Thêm gói kit vào giỏ hàng thành công!");
                }
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Thêm gói kit vào giỏ hàng thất bại!")
                    .AddError("outOfService", "Không thể thêm vào giỏ hàng ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetByIdAsync(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status401Unauthorized)
                        .AddDetail("message", "Lấy giỏ hàng thất bại!")
                        .AddError("notFound", "Không tìm thấy tài khoản ngay lúc này!");
                }

                var (carts, totalPages) = await _unitOfWork.CartRepository.GetFilterAsync(
                    u => u.UserId == user.Id,
                    includes: new Func<IQueryable<Cart>, IQueryable<Cart>>[]
                    {
                        c => c.Include(p => p.Package)
                    });

                if (!carts.Any())
                {
                    return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Lấy giỏ hàng thành công!")
                        .AddDetail("data", "Giỏ hàng của bạn đang trống!");
                }

                // Map carts to DTO
                var cartDTOs = _mapper.Map<IEnumerable<CartResponseDTO>>(carts);

                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy giỏ hàng thành công!")
                    .AddDetail("data", new { carts = cartDTOs });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy giỏ hàng thất bại!")
                    .AddError("outOfService", "Không thể lấy giỏ hàng ngay lúc này!");
            }
        }


        public async Task<ServiceResponse> RemoveAllAsync(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status401Unauthorized)
                        .AddDetail("message", "Xóa gói kit ra khỏi giỏ hàng thất bại!")
                        .AddError("notFound", "Không tìm thấy tài khoản ngay lúc này!");
                }

                var cartItems = await _unitOfWork.CartRepository.GetFilterAsync(
                    c => c.UserId == user.Id);

                if (!cartItems.Item1.Any())
                {
                    return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Giỏ hàng của bạn đang trống!");
                }

                foreach (var item in cartItems.Item1)
                {
                    await _unitOfWork.CartRepository.RemoveAsync(item);
                }

                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Xóa giỏ hàng thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Xóa giỏ hàng thất bại!")
                    .AddError("outOfService", "Không thể xóa giỏ hàng ngay lúc này!");
            }

        }

        public async Task<ServiceResponse> RemoveByPackageIdAsync(string userName, int packageId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status401Unauthorized)
                        .AddDetail("message", "Xóa gói kit ra khỏi giỏ hàng thất bại!")
                        .AddError("notFound", "Không tìm thấy tài khoản ngay lúc này!");
                }

                var existingCart = await _unitOfWork.CartRepository.GetFilterAsync(
                   c => c.UserId == user.Id && c.PackageId == packageId);

                if (!existingCart.Item1.Any())
                {
                    return new ServiceResponse()
                         .SetSucceeded(false)
                         .AddDetail("message", "Xóa gói kit ra khỏi giỏ hàng thất bại!")
                         .AddError("notFounde", "Không tìm thấy gói kit ngay lúc này!");
                }

                var cart = existingCart.Item1.First();
                await _unitOfWork.CartRepository.RemoveAsync(cart);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Xóa gói kit ra khỏi giỏ hàng thành công!");
            }
            catch
            {
                return new ServiceResponse()
                   .SetSucceeded(false)
                   .AddDetail("message", "Xóa gói kit ra khỏi giỏ hàng thất bại!")
                   .AddError("outOfService", "Không thể xóa gói kit ra khỏi giỏ hàng ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(string userName, CartDTO cartDTO)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status401Unauthorized)
                        .AddDetail("message", "Chỉnh sửa giỏ hàng thất bại!")
                        .AddError("notFound", "Không tìm thấy tài khoản ngay lúc này!");
                }

                var existingCart = await _unitOfWork.CartRepository.GetFilterAsync(
                    c => c.UserId == user.Id && c.PackageId == cartDTO.PackageId);

                if (!existingCart.Item1.Any())
                {
                    return new ServiceResponse()
                         .SetSucceeded(false)
                         .SetStatusCode(StatusCodes.Status404NotFound)
                         .AddDetail("message", "Chỉnh sửa giỏ hàng thất bại!")
                         .AddError("notFound", "Không tìm thấy gói kit ngay lúc này!");
                }

                var cart = existingCart.Item1.First();
                cart.PackageQuantity = cartDTO.PackageQuantity;
                await _unitOfWork.CartRepository.UpdateAsync(cart);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Chỉnh sửa số lượng gói kit trong giỏ hàng thành công!");
            }
            catch
            {
                return new ServiceResponse()
                   .SetSucceeded(false)
                   .AddDetail("message", "Chỉnh sửa giỏ hàng thất bại!")
                   .AddError("outOfService", "Không thể chỉnh sửa gói kit vào giỏ hàng ngay lúc này!");
            }
        }
    }
}
