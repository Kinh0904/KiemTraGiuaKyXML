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
}
catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or XmlException)
{
    Console.Error.WriteLine($"Không thể đọc danh sách nhân viên: {ex.Message}");
    Environment.ExitCode = 1;
}
