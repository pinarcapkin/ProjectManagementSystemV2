using System.Threading.Tasks;
using ProjectManagementSystemV2.Entities.DTOs; // RegisterDto ve LoginDto sınıflarının bulunduğu doğru namespace
using ProjectManagementSystemV2.Entities; // User tablosu (entity) sınıfının bulunduğu namespace
using ProjectManagementSystemV2.Repositories; // GenericRepository ve UnitOfWork arayüzlerinin bulunduğu namespace

namespace ProjectManagementSystemV2.Services;

// Kullanıcıya özel iş kurallarını (Kayıt olma, Giriş yapma vb.) yöneten servis sınıfı
public class UserService : IUserService
{
    private readonly IGenericRepository<User> _userRepository; // User tablosunda veritabanı sorguları yapmak için
    private readonly IUnitOfWork _unitOfWork; // Yapılan değişiklikleri veritabanına kalıcı olarak kaydetmek (commit) için

    // Constructor (Yapıcı Metot): Dependency Injection ile repository ve unitofwork nesnelerini içeri alıyoruz
    public UserService(IGenericRepository<User> userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    // Kullanıcı kayıt olma işlemini gerçekleştiren metot
    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        // 1. Adım: Bu e-posta adresiyle daha önce sisteme kayıt olunmuş mu kontrol ediyoruz
        var existingUser = await _userRepository.AnyAsync(x => x.Email == registerDto.Email);
        if (existingUser)
        {
            return false; // Eğer bu mail zaten varsa, kayıt başarısız (false) döner
        }

        // 2. Adım: Dışarıdan gelen DTO verisini, veritabanı tablomuz olan User modeline dönüştürüyoruz
        var newUser = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            PasswordHash = registerDto.Password, // Burayı PasswordHash yaptık
            Department = registerDto.Department
        };

        // 3. Adım: Yeni kullanıcıyı veritabanına ekleme sırasına koyuyoruz ve UnitOfWork ile kalıcı olarak kaydediyoruz
        await _userRepository.AddAsync(newUser);
        await _unitOfWork.CommitAsync();

        return true; // Kayıt işlemi başarılı bir şekilde tamamlandı
    }

    // Kullanıcı giriş yapma (Login) işlemini gerçekleştiren metot
    public async Task<bool> LoginAsync(LoginDto loginDto)
    {
        // Veritabanında hem girilen e-postaya hem de şifreye sahip kullanıcı var mı diye bakıyoruz
        var userExists = await _userRepository.AnyAsync(x => x.Email == loginDto.Email && x.PasswordHash == loginDto.Password); // Burayı da PasswordHash yaptık
        
        return userExists; 
    }
}