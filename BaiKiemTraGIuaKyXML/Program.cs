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
                               ConLamViec = (bool?)nhanVien.Element("ConLamViec") == true,
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

    // Câu 5: Sắp xếp tuổi giảm dần.
    var danhSachSapXep = danhSachThongTin.OrderByDescending(nv => nv.Tuoi);
    Console.WriteLine("\nCÂU 5 - NHÂN VIÊN THEO TUỔI GIẢM DẦN");
    foreach (var nv in danhSachSapXep)
        Console.WriteLine($"{nv.MaNV} | {nv.HoTen} | {nv.BoPhan} | {nv.Tuoi} tuổi");

    // Câu 6: Hiển thị tất cả nhân viên đồng tuổi cao nhất, nếu có.
    Console.WriteLine("\nCÂU 6 - NHÂN VIÊN CÓ TUỔI CAO NHẤT");
    if (danhSachThongTin.Count == 0)
        Console.WriteLine("Danh sách nhân viên trống.");
    else
    {
        int tuoiCaoNhat = danhSachThongTin.Max(nv => nv.Tuoi);
        var nhanVienCaoTuoiNhat = danhSachThongTin.Where(nv => nv.Tuoi == tuoiCaoNhat);
        foreach (var nv in nhanVienCaoTuoiNhat)
            Console.WriteLine($"{nv.MaNV} | {nv.HoTen} | {nv.NgheNghiep} | {nv.Tuoi} tuổi");
    }

    // Câu 7: Tuổi trung bình của từng nhóm giới tính.
    var thongKeGioiTinh = danhSachThongTin
        .Where(nv => nv.GioiTinh == "Nam" || nv.GioiTinh == "Nữ")
        .GroupBy(nv => nv.GioiTinh)
        .Select(nhom => new
        {
            GioiTinh = nhom.Key,
            TuoiTrungBinh = nhom.Average(nv => nv.Tuoi)
        }).ToList();

    Console.WriteLine("\nCÂU 7 - TUỔI TRUNG BÌNH CỦA NAM VÀ NỮ");
    foreach (string gioiTinh in new[] { "Nam", "Nữ" })
    {
        var nhom = thongKeGioiTinh.FirstOrDefault(n => n.GioiTinh == gioiTinh);
        Console.WriteLine(nhom == null
            ? $"{gioiTinh}: Không có nhân viên."
            : $"{gioiTinh}: {nhom.TuoiTrungBinh:F2} tuổi");
    }

    // Câu 8: Tuổi trung bình tính trên toàn bộ nhóm giới tính (kể cả người đã nghỉ).
    // Sau đó đếm người còn làm việc thuộc các nhóm có tuổi trung bình >= 20.
    int soNhanVienConLamViec = danhSachThongTin.Count(nv => nv.ConLamViec
        && thongKeGioiTinh.Any(nhom => nhom.GioiTinh == nv.GioiTinh && nhom.TuoiTrungBinh >= 20));
    Console.WriteLine("\nCÂU 8 - ĐẾM NHÂN VIÊN CÒN LÀM VIỆC THUỘC NHÓM GIỚI TÍNH CÓ TUỔI TRUNG BÌNH >= 20");
    Console.WriteLine($"Số nhân viên: {soNhanVienConLamViec}");

    // Câu 9: Đếm số nhân viên theo từng bộ phận.
    var thongKeBoPhan = danhSachThongTin.GroupBy(nv => nv.BoPhan)
        .Select(nhom => new { BoPhan = nhom.Key, SoLuong = nhom.Count() });
    Console.WriteLine("\nCÂU 9 - SỐ LƯỢNG NHÂN VIÊN THEO BỘ PHẬN");
    foreach (var nhom in thongKeBoPhan)
        Console.WriteLine($"{nhom.BoPhan}: {nhom.SoLuong} nhân viên");
}
catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or XmlException)
{
    Console.Error.WriteLine($"Không thể đọc danh sách nhân viên: {ex.Message}");
    Environment.ExitCode = 1;
}
