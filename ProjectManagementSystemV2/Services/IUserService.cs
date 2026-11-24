using System.Threading.Tasks;
using ProjectManagementSystemV2.Entities.DTOs; // Doğru namespace'i buraya ekledik

namespace ProjectManagementSystemV2.Services;

public interface IUserService
{
    // Kullanıcı kayıt işlemi (Gelen RegisterDto verisiyle yeni kullanıcı oluşturur)
    Task<bool> RegisterAsync(RegisterDto registerDto);

    // Kullanıcı giriş işlemi (Giriş başarılı ise true döner)
    Task<bool> LoginAsync(LoginDto loginDto);
}