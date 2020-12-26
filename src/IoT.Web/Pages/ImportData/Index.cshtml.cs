using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;

using System.Threading.Tasks;
using IoT.BTS.Countrys;
using IoT.BTS.InforTrams;
using IoT.BTS.InforTruAntens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using IoT.BTS.ChuSoHuus;
using System.Text.RegularExpressions;

namespace IoT.Web.Pages.ImportData
{
    public class IndexModel : AbpPageModel
    {
        [BindProperty]
        public InputFileDto Input { get; set; }

        public List<SelectListItem> ListCity { get; set; }

        private readonly IRepository<IoT.BTS.InforTrams.LoaiTram, Guid> _loaitramRepository;

        private readonly IRepository<Country, Guid> _countryRepository;

        private readonly IRepository<InforTruAnten, Guid> _inforTruAntens;

        private readonly IRepository<InforTram, Guid> _infortramRepository;

        private readonly IRepository<ChuSoHuu, Guid> _chuSoHuuRepository;
        public IndexModel(IRepository<Country, Guid> countryRepository,
                          IRepository<IoT.BTS.InforTrams.LoaiTram, Guid> loaitramRepository,
                          IRepository<InforTruAnten, Guid> inforTruAntens,
                          IRepository<InforTram, Guid> infortramRepository,
                          IRepository<ChuSoHuu, Guid> chuSoHuuRepository)
        {
            _countryRepository = countryRepository;
            _loaitramRepository = loaitramRepository;
            _inforTruAntens = inforTruAntens;
            _infortramRepository = infortramRepository;
            _chuSoHuuRepository = chuSoHuuRepository;
        }

        public async Task OnGet()
        {
            Input = new InputFileDto();
            //var lstCountry = await _countryRepository.GetListAsync();
            //lstCountry = lstCountry.Where(x => x.level == 1).ToList();
            //ListCity = lstCountry.Select(m => new SelectListItem() { Value = m.code, Text = m.name }).ToList();
            ListCity = new List<SelectListItem>();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            var checkOutDiaChi = new List<string>();
            var lstError = new List<int>();

            ListCity = new List<SelectListItem>();
            var tp = new Country();
            try
            {
                if (Input.Files != null)
                {
                    string fileExtension = Path.GetExtension(Input.Files.FileName);
                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        using (var stream = new MemoryStream())
                        {
                            await Input.Files.CopyToAsync(stream);

                            using (ExcelPackage package = new ExcelPackage(stream))
                            {
                                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                                ExcelWorksheet workSheet = package.Workbook.Worksheets.ElementAt(0);
                                //var workSheet = package.Workbook.Worksheets.First();
                                int totalRows = workSheet.Dimension.Rows;

                                var lstCountry = await _countryRepository.GetListAsync();

                                tp = lstCountry.Find(x => x.code == Input.CityCode); // thành phố

                                var lstLoaiTram = await _loaitramRepository.GetListAsync(); // danh sách loại trạm

                                var vitridialy = string.Empty; // vị trí đọc từ excel (xã, phường, thị trấn)

                                var huyenCode = string.Empty;

                                var lstChuSoHuu = await this._chuSoHuuRepository.GetListAsync(); // danh sách chủ sở hữu
                                                                                                 // danh sách input
                                var lstInfoTram = new List<InforTram>();
                                var lstInfoTruAnten = new List<InforTruAnten>();

                                var numcot = -1;

                                for (int i = Input.StartNumber; i <= Input.EndNumber; i++)
                                {
                                    var cot = workSheet.Cells[i, 1].Value == null ? "" : workSheet.Cells[i, 1].Value.ToString();

                                    try
                                    {
                                        if (!Int32.TryParse(cot, out numcot)) // đọc dòng huyện
                                        {
                                            var isHuyen = isRoman(cot);
                                            if (isHuyen)
                                            {
                                                var cothuyen = workSheet.Cells[i, 2].Value == null ? "" : workSheet.Cells[i, 2].Value.ToString();
                                                cothuyen = cothuyen.Replace(Environment.NewLine, "");
                                                cothuyen = cothuyen.Replace("\n", " ");

                                                var diachiHuyen = lstCountry.Find(x => (x.description.Trim().ToLower() + " " + x.name.Trim().ToLower()) == cothuyen.Trim().ToLower()
                                                                               && x.level == 2
                                                                               && !string.IsNullOrEmpty(x.code)
                                                                               && x.code.Contains(Input.CityCode));

                                                if (diachiHuyen != null)
                                                {
                                                    huyenCode = diachiHuyen.code;
                                                }
                                            }
                                        }

                                        var viTri = workSheet.Cells[i, 4].Value == null ? "" : workSheet.Cells[i, 4].Value.ToString();

                                        if (!string.IsNullOrEmpty(viTri)) // đọc dòng xã phường
                                        {
                                            vitridialy = viTri;
                                        }

                                        var columDiaChi = workSheet.Cells[i, 3].Value == null ? "" : workSheet.Cells[i, 3].Value.ToString();

                                        if (!string.IsNullOrEmpty(columDiaChi)) // đọc dòng data
                                        {
                                            var codeCheck = Input.CityCode;
                                            if (!string.IsNullOrEmpty(huyenCode))
                                            {
                                                codeCheck = huyenCode;
                                            }
                                            vitridialy = vitridialy.Replace(Environment.NewLine, " ");
                                            vitridialy = vitridialy.Replace("\n", " ");

                                            var diachi = lstCountry.Find(x => (x.description.Trim().ToLower() + " " + x.name.Trim().ToLower()) == vitridialy.Trim().ToLower()
                                                                                && x.level == 3
                                                                                && !string.IsNullOrEmpty(x.code)
                                                                                && x.code.Contains(codeCheck));


                                            if (diachi == null)
                                            {
                                                if (!checkOutDiaChi.Contains(vitridialy))
                                                {
                                                    checkOutDiaChi.Add(vitridialy);
                                                }
                                            }

                                            // thông tin trụ ăng ten
                                            var kinhDo = workSheet.Cells[i, 5].Value == null ? "" : workSheet.Cells[i, 5].Value.ToString();
                                            var viDo = workSheet.Cells[i, 6].Value == null ? "" : workSheet.Cells[i, 6].Value.ToString();
                                            var chuSoHuu = workSheet.Cells[i, 7].Value == null ? "" : workSheet.Cells[i, 7].Value.ToString();
                                            var namXayDung = workSheet.Cells[i, 8].Value == null ? "" : workSheet.Cells[i, 8].Value.ToString();
                                            var a1A = workSheet.Cells[i, 9].Value == null ? "" : workSheet.Cells[i, 9].Value.ToString();
                                            var a1B = workSheet.Cells[i, 10].Value == null ? "" : workSheet.Cells[i, 10].Value.ToString();
                                            var a2A = workSheet.Cells[i, 11].Value == null ? "" : workSheet.Cells[i, 11].Value.ToString();
                                            var a2B = workSheet.Cells[i, 12].Value == null ? "" : workSheet.Cells[i, 12].Value.ToString();
                                            var a2C = workSheet.Cells[i, 13].Value == null ? "" : workSheet.Cells[i, 13].Value.ToString();
                                            var kieuCot = workSheet.Cells[i, 14].Value == null ? "" : workSheet.Cells[i, 14].Value.ToString();
                                            var chieuCao = workSheet.Cells[i, 16].Value == null ? "" : workSheet.Cells[i, 16].Value.ToString();
                                            var baoTri = workSheet.Cells[i, 17].Value == null ? "" : workSheet.Cells[i, 17].Value.ToString();

                                            var loaiCot = new LoaiCot()
                                            {
                                                CongKenhA1a = (!string.IsNullOrEmpty(a1A) ? true : false),
                                                CongKenhA1b = (!string.IsNullOrEmpty(a1B) ? true : false),
                                                CongKenhA2a = (!string.IsNullOrEmpty(a2A) ? true : false),
                                                CongKenhA2b = (!string.IsNullOrEmpty(a2B) ? true : false),
                                                CongKenhA2c = (!string.IsNullOrEmpty(a2C) ? true : false)
                                            };

                                            var chuSoHuuDto = lstChuSoHuu.FindAll(x => x.Ten.Trim().ToLower() == chuSoHuu.Trim().ToLower()).FirstOrDefault();

                                            var lat = 0.0;
                                            var lng = 0.0;
                                            if (!string.IsNullOrEmpty(viDo))
                                            {
                                                viDo = viDo.Replace(",", ".");

                                                lat = double.Parse(viDo, CultureInfo.GetCultureInfo("en"));
                                            }

                                            if (!string.IsNullOrEmpty(kinhDo))
                                            {
                                                kinhDo = kinhDo.Replace(",", ".");

                                                lng = double.Parse(kinhDo, CultureInfo.GetCultureInfo("en"));
                                            }
                                            var newTruAnTen = new InforTruAnten()
                                            {
                                                DiaChi = columDiaChi,
                                                ContryCode = (diachi != null ? diachi.fullCode : !string.IsNullOrEmpty(huyenCode) ? huyenCode : Input.CityCode),
                                                Lat = lat,
                                                Lng = lng,
                                                ChuSoHuu = (chuSoHuuDto != null ? chuSoHuuDto.Id.ToString() : string.Empty),
                                                NamXayDung = namXayDung,
                                                KieuCot = (!string.IsNullOrEmpty(kieuCot) ? true : false),
                                                ChieuCao = Convert.ToDouble(chieuCao),
                                                CongTacKiemDinh = (!string.IsNullOrEmpty(baoTri) ? true : false),
                                                LoaiCotAnTen = loaiCot
                                            };

                                            var infortruAngten = await this._inforTruAntens.InsertAsync(newTruAnTen);

                                            if (infortruAngten != null)
                                            {
                                                lstInfoTruAnten.Add(infortruAngten);
                                            }

                                            // thông tin của trạm 2G
                                            var maTram2G = workSheet.Cells[i, 19].Value == null ? "" : workSheet.Cells[i, 19].Value.ToString();
                                            var tenTram2G = workSheet.Cells[i, 20].Value == null ? "" : workSheet.Cells[i, 20].Value.ToString();
                                            var tenThietBi2G = workSheet.Cells[i, 21].Value == null ? "" : workSheet.Cells[i, 21].Value.ToString();
                                            var nuocSX2G = workSheet.Cells[i, 22].Value == null ? "" : workSheet.Cells[i, 22].Value.ToString();
                                            var batBuocKiemDinh2G = workSheet.Cells[i, 23].Value == null ? "" : workSheet.Cells[i, 23].Value.ToString();
                                            var ngayPhatSong2G = workSheet.Cells[i, 25].Value == null ? "" : workSheet.Cells[i, 25].Value.ToString();
                                            var congSuatTram2G = workSheet.Cells[i, 26].Value == null ? "" : workSheet.Cells[i, 26].Value.ToString();
                                            var phamViPhatSong2G = workSheet.Cells[i, 27].Value == null ? "" : workSheet.Cells[i, 27].Value.ToString();
                                            var heThongTruyenDan2G = workSheet.Cells[i, 28].Value == null ? "" : workSheet.Cells[i, 28].Value.ToString();

                                            if (!string.IsNullOrEmpty(ngayPhatSong2G))
                                            {
                                                var splitDate2G = ngayPhatSong2G.Split(" ");
                                                ngayPhatSong2G = splitDate2G.ElementAt(0);
                                            }

                                            var date2G = DateTime.Now;
                                            try // check ngay
                                            {
                                                date2G = (!string.IsNullOrEmpty(ngayPhatSong2G) ? DateTime.ParseExact(ngayPhatSong2G, "dd/MM/yyyy", null) : DateTime.Now);
                                            }
                                            catch (Exception ex)
                                            {
                                                date2G = DateTime.Now;
                                                lstError.Add(i);
                                            }

                                            if (!string.IsNullOrEmpty(maTram2G))
                                            {
                                                var loaitram2g = lstLoaiTram.Find(x => x.TenLoaiTram == "Trạm 2G");

                                                var tram2G = new InforTram()
                                                {
                                                    IdInforTru = (infortruAngten != null ? infortruAngten.Id.ToString() : string.Empty),
                                                    IdLoaiTram = (loaitram2g != null ? loaitram2g.Id.ToString() : string.Empty),
                                                    MaTram = maTram2G,
                                                    TenTram = tenTram2G,
                                                    TenThietBi = tenThietBi2G,
                                                    NuocSanXuat = nuocSX2G,
                                                    BatBuotKiemDinh = (!string.IsNullOrEmpty(batBuocKiemDinh2G) ? true : false),
                                                    NgayPhatSong = date2G,
                                                    CongSuatTram = congSuatTram2G,
                                                    PhamViPhatSong = phamViPhatSong2G,
                                                    HeThongTruyenDan = (!string.IsNullOrEmpty(heThongTruyenDan2G) ? true : false)
                                                };

                                                var infortram2g = await this._infortramRepository.InsertAsync(tram2G);
                                                if (infortram2g != null)
                                                {
                                                    lstInfoTram.Add(infortram2g);
                                                }
                                            }

                                            // thông tin của trạm 3G
                                            var maTram3G = workSheet.Cells[i, 30].Value == null ? "" : workSheet.Cells[i, 30].Value.ToString();
                                            var tenTram3G = workSheet.Cells[i, 31].Value == null ? "" : workSheet.Cells[i, 31].Value.ToString();
                                            var tenThietBi3G = workSheet.Cells[i, 32].Value == null ? "" : workSheet.Cells[i, 32].Value.ToString();
                                            var nuocSX3G = workSheet.Cells[i, 33].Value == null ? "" : workSheet.Cells[i, 33].Value.ToString();
                                            var batBuocKiemDinh3G = workSheet.Cells[i, 34].Value == null ? "" : workSheet.Cells[i, 34].Value.ToString();
                                            var ngayPhatSong3G = workSheet.Cells[i, 36].Value == null ? "" : workSheet.Cells[i, 36].Value.ToString();
                                            var congSuatTram3G = workSheet.Cells[i, 37].Value == null ? "" : workSheet.Cells[i, 37].Value.ToString();
                                            var phamViPhatSong3G = workSheet.Cells[i, 38].Value == null ? "" : workSheet.Cells[i, 38].Value.ToString();
                                            var heThongTruyenDan3G = workSheet.Cells[i, 39].Value == null ? "" : workSheet.Cells[i, 39].Value.ToString();

                                            if (!string.IsNullOrEmpty(ngayPhatSong3G))
                                            {
                                                var splitDate3G = ngayPhatSong3G.Split(" ");
                                                ngayPhatSong3G = splitDate3G.ElementAt(0);
                                            }

                                            var date3G = DateTime.Now;
                                            try // check ngay
                                            {
                                                date3G = (!string.IsNullOrEmpty(ngayPhatSong3G) ? DateTime.ParseExact(ngayPhatSong3G, "dd/MM/yyyy", null) : DateTime.Now);
                                            }
                                            catch (Exception ex)
                                            {
                                                date3G = DateTime.Now;

                                                if (lstError.IndexOf(i) < 0)
                                                {
                                                    lstError.Add(i);
                                                }
                                            }


                                            if (!string.IsNullOrEmpty(maTram3G))
                                            {
                                                var loaitram3g = lstLoaiTram.Find(x => x.TenLoaiTram == "Trạm 3G");

                                                var tram3G = new InforTram()
                                                {
                                                    IdInforTru = (infortruAngten != null ? infortruAngten.Id.ToString() : string.Empty),
                                                    IdLoaiTram = (loaitram3g != null ? loaitram3g.Id.ToString() : string.Empty),
                                                    MaTram = maTram3G,
                                                    TenTram = tenTram3G,
                                                    TenThietBi = tenThietBi3G,
                                                    NuocSanXuat = nuocSX3G,
                                                    BatBuotKiemDinh = (!string.IsNullOrEmpty(batBuocKiemDinh3G) ? true : false),
                                                    NgayPhatSong = date3G,
                                                    CongSuatTram = congSuatTram3G,
                                                    PhamViPhatSong = phamViPhatSong3G,
                                                    HeThongTruyenDan = (!string.IsNullOrEmpty(heThongTruyenDan3G) ? true : false)
                                                };

                                                var infortram3g = await this._infortramRepository.InsertAsync(tram3G);
                                                if (infortram3g != null)
                                                {
                                                    lstInfoTram.Add(infortram3g);
                                                }
                                            }

                                            // thông tin của trạm 4G
                                            var maTram4G = workSheet.Cells[i, 41].Value == null ? "" : workSheet.Cells[i, 41].Value.ToString();
                                            var tenTram4G = workSheet.Cells[i, 42].Value == null ? "" : workSheet.Cells[i, 42].Value.ToString();
                                            var tenThietBi4G = workSheet.Cells[i, 43].Value == null ? "" : workSheet.Cells[i, 43].Value.ToString();
                                            var nuocSX4G = workSheet.Cells[i, 44].Value == null ? "" : workSheet.Cells[i, 44].Value.ToString();
                                            var batBuocKiemDinh4G = workSheet.Cells[i, 45].Value == null ? "" : workSheet.Cells[i, 45].Value.ToString();
                                            var ngayPhatSong4G = workSheet.Cells[i, 47].Value == null ? "" : workSheet.Cells[i, 47].Value.ToString();
                                            var congSuatTram4G = workSheet.Cells[i, 48].Value == null ? "" : workSheet.Cells[i, 48].Value.ToString();
                                            var phamViPhatSong4G = workSheet.Cells[i, 49].Value == null ? "" : workSheet.Cells[i, 49].Value.ToString();
                                            var heThongTruyenDan4G = workSheet.Cells[i, 50].Value == null ? "" : workSheet.Cells[i, 50].Value.ToString();

                                            if (!string.IsNullOrEmpty(ngayPhatSong4G))
                                            {
                                                var splitDate4G = ngayPhatSong4G.Split(" ");
                                                ngayPhatSong4G = splitDate4G.ElementAt(0);
                                            }

                                            var date4G = DateTime.Now;
                                            try // check ngay
                                            {
                                                date4G = (!string.IsNullOrEmpty(ngayPhatSong4G) ? DateTime.ParseExact(ngayPhatSong4G, "dd/MM/yyyy", null) : DateTime.Now);
                                            }
                                            catch (Exception ex)
                                            {
                                                date4G = DateTime.Now;

                                                if (lstError.IndexOf(i) < 0)
                                                {
                                                    lstError.Add(i);
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(maTram4G))
                                            {
                                                var loaitram4g = lstLoaiTram.Find(x => x.TenLoaiTram == "Trạm 4G");

                                                var tram4G = new InforTram()
                                                {
                                                    IdInforTru = (infortruAngten != null ? infortruAngten.Id.ToString() : string.Empty),
                                                    IdLoaiTram = (loaitram4g != null ? loaitram4g.Id.ToString() : string.Empty),
                                                    MaTram = maTram4G,
                                                    TenTram = tenTram4G,
                                                    TenThietBi = tenThietBi4G,
                                                    NuocSanXuat = nuocSX4G,
                                                    BatBuotKiemDinh = (!string.IsNullOrEmpty(batBuocKiemDinh4G) ? true : false),
                                                    NgayPhatSong = date4G,
                                                    CongSuatTram = congSuatTram4G,
                                                    PhamViPhatSong = phamViPhatSong4G,
                                                    HeThongTruyenDan = (!string.IsNullOrEmpty(heThongTruyenDan4G) ? true : false)
                                                };

                                                var infortram4g = await this._infortramRepository.InsertAsync(tram4G);
                                                if (infortram4g != null)
                                                {
                                                    lstInfoTram.Add(infortram4g);
                                                }
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }

                //if (checkOutDiaChi.Count() > 0)
                //{
                //    Alerts.Warning("Không tìm thấy địa chỉ " + string.Join(",", checkOutDiaChi));
                //}

                if (checkOutDiaChi.Count() > 0)
                {
                    TempData["Message"] = "Không tìm thấy địa chỉ " + string.Join(",", checkOutDiaChi) + " (Nên đã đổi thành " + (tp.Id != null ? tp.name : "") + ") " +
                                (lstError.Count() > 0 ? "và lỗi ngày tháng năm ở các dòng " + string.Join(", ", lstError) + " (Nên đã đổi thành ngày hiện tại)" : "");
                    TempData["MessageType"] = "alert-warning";
                }
                else
                {
                    TempData["Message"] = "Thêm dữ liệu thành công";
                    TempData["MessageType"] = "alert-success";
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToPage("./Index");

        }

        public bool isRoman(String s)
        {
            bool isMatch = Regex.IsMatch(s, "M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})",
                                         RegexOptions.IgnoreCase);

            return !string.IsNullOrEmpty(s)
                   && isMatch;
        }

    }
}

public class InputFileDto
{
    public IFormFile Files { get; set; }

    [Required]
    public int StartNumber { get; set; }

    [Required]
    public int EndNumber { get; set; }

    [Required]
    public string CityCode { get; set; }
}

