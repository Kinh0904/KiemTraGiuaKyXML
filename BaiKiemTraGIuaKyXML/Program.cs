using System.Text;
using System.Xml;
using System.Xml.Linq;

Console.OutputEncoding = Encoding.UTF8;

// Data.xml được sao chép từ thư mục chứa solution vào thư mục chạy ứng dụng.
string duongDan = Path.Combine(AppContext.BaseDirectory, "Data.xml");

try
{
    XDocument taiLieu = XDocument.Load(duongDan);
    XElement goc = taiLieu.Element("DanhSachNhanVien")
        ?? throw new InvalidDataException("XML phải có phần tử gốc DanhSachNhanVien.");
    var danhSachNhanVien = goc.Elements("NhanVien").ToList();

    Console.WriteLine("DANH SÁCH NHÂN VIÊN");
    Console.WriteLine($"Tổng số nhân viên: {danhSachNhanVien.Count}");

    if (danhSachNhanVien.Count == 0)
    {
        Console.WriteLine("Danh sách nhân viên trống.");
    }

    foreach (XElement nhanVien in danhSachNhanVien)
    {
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"Mã nhân viên: {nhanVien.Element("MaNV")?.Value}");
        Console.WriteLine($"Họ tên:       {nhanVien.Element("HoTen")?.Value}");
        Console.WriteLine($"Bộ phận:      {nhanVien.Element("BoPhan")?.Value}");
        Console.WriteLine($"Giới tính:    {nhanVien.Element("GioiTinh")?.Value}");
        Console.WriteLine($"Ngày sinh:    {nhanVien.Element("NgaySinh")?.Value}");
        Console.WriteLine($"Nghề nghiệp:  {nhanVien.Element("NgheNghiep")?.Value}");
        Console.WriteLine($"Quê quán:     {nhanVien.Element("QueQuan")?.Value}");
    }

    // Câu 2: Dùng LINQ lấy mã nhân viên, họ tên và bộ phận.
    var danhSachRutGon = from nhanVien in danhSachNhanVien
                        select new
                        {
                            MaNV = nhanVien.Element("MaNV")?.Value,
                            HoTen = nhanVien.Element("HoTen")?.Value,
                            BoPhan = nhanVien.Element("BoPhan")?.Value
                        };

    Console.WriteLine("\nCÂU 2 - DANH SÁCH MÃ NHÂN VIÊN, HỌ TÊN VÀ BỘ PHẬN");
    foreach (var nhanVien in danhSachRutGon)
    {
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"Mã nhân viên: {nhanVien.MaNV}");
        Console.WriteLine($"Họ tên:       {nhanVien.HoTen}");
        Console.WriteLine($"Bộ phận:      {nhanVien.BoPhan}");
    }

    // Câu 3: Dùng LINQ tìm nhân viên từ đủ 20 tuổi đến hết 35 tuổi.
    DateTime homNay = DateTime.Today;
    // Tính tuổi một lần để dùng chung cho các truy vấn.
    var danhSachThongTin = (from nhanVien in danhSachNhanVien
                           let ngaySinh = (DateTime)nhanVien.Element("NgaySinh")!
                           let tuoi = homNay.Year - ngaySinh.Year
                               - (ngaySinh.Date.AddYears(homNay.Year - ngaySinh.Year) > homNay ? 1 : 0)
                           select new
                           {
                               MaNV = nhanVien.Element("MaNV")?.Value,
                               HoTen = nhanVien.Element("HoTen")?.Value,
                               BoPhan = nhanVien.Element("BoPhan")?.Value,
                               GioiTinh = nhanVien.Element("GioiTinh")?.Value,
                               NgheNghiep = nhanVien.Element("NgheNghiep")?.Value,
                               QueQuan = nhanVien.Element("QueQuan")?.Value,
                               NgaySinh = ngaySinh,
                               Tuoi = tuoi
                           }).ToList();
    var danhSachTheoTuoi = danhSachThongTin
        .Where(nhanVien => nhanVien.Tuoi >= 20 && nhanVien.Tuoi <= 35).ToList();

    Console.WriteLine("\nCÂU 3 - DANH SÁCH NHÂN VIÊN TỪ 20 ĐẾN 35 TUỔI");
    Console.WriteLine($"Số nhân viên phù hợp: {danhSachTheoTuoi.Count}");
    if (danhSachTheoTuoi.Count == 0)
    {
        Console.WriteLine("Không có nhân viên trong độ tuổi từ 20 đến 35.");
    }

    foreach (var nhanVien in danhSachTheoTuoi)
    {
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"Mã nhân viên: {nhanVien.MaNV}");
        Console.WriteLine($"Họ tên:       {nhanVien.HoTen}");
        Console.WriteLine($"Bộ phận:      {nhanVien.BoPhan}");
        Console.WriteLine($"Ngày sinh:    {nhanVien.NgaySinh:dd/MM/yyyy}");
        Console.WriteLine($"Tuổi:         {nhanVien.Tuoi}");
    }
    // Câu 4: Kết hợp các điều kiện bằng toán tử &&.
    var danhSachNhieuDieuKien = danhSachThongTin.Where(nv =>
        nv.Tuoi >= 25 && nv.GioiTinh == "Nam" && nv.BoPhan == "Công nghệ thông tin").ToList();

    Console.WriteLine("\nCÂU 4 - NAM TỪ 25 TUỔI THUỘC BỘ PHẬN CÔNG NGHỆ THÔNG TIN");
    if (danhSachNhieuDieuKien.Count == 0)
        Console.WriteLine("Không có nhân viên thỏa mãn điều kiện.");
    foreach (var nv in danhSachNhieuDieuKien)
        Console.WriteLine($"{nv.MaNV} | {nv.HoTen} | {nv.BoPhan} | {nv.GioiTinh} | {nv.Tuoi} tuổi");
}
catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or XmlException)
{
    Console.Error.WriteLine($"Không thể đọc danh sách nhân viên: {ex.Message}");
    Environment.ExitCode = 1;
}
