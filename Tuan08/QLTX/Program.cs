using System;
using System.Collections.Generic;
using System.Linq;
using ThucHanh9.Models;

namespace ThucHanh9
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Ui.Start("QLTX · Quản lý thuê xe tự lái",
                new Entry("Quản lý xe", store => new CatalogForm<Xe>(store, options: new CatalogOptions<Xe>
                {
                    Search = new[] { "Maxe", "HangSX", "Sokhung", "Somay" },
                    Validate = (session, row, old) =>
                    {
                        if (row.NamSX > DateTime.Today.Year)
                            throw new InvalidOperationException("Năm sản xuất không thể lớn hơn năm hiện tại.");
                        if (old != null)
                        {
                            DateTime today = DateTime.Today;
                            List<YeuCau> future = session.List<YeuCau>(request => request.Maxe == row.Maxe && request.Ngaytra >= today);
                            if (PeakUse(future) > row.SoLuong)
                                throw new InvalidOperationException("Số xe ít hơn số lượng đã được đặt.");
                        }
                    }
                })),
                new Entry("Quản lý khách hàng", store => new CatalogForm<KhachHang>(store, options: new CatalogOptions<KhachHang>
                {
                    Search = new[] { "MaK", "TenK", "Tel" }
                })),
                new Entry("Quản lý yêu cầu thuê xe", store => new CatalogForm<YeuCau>(store, options: new CatalogOptions<YeuCau>
                {
                    Search = new[] { "MaK", "Maxe" }, Filter = "Maxe",
                    Defaults = () => new YeuCau { Ngaymuon = DateTime.Today, Ngaytra = DateTime.Today.AddDays(1) },
                    Validate = CheckReservation
                })));
        }

        private static void CheckReservation(DataSession session, YeuCau row, YeuCau original)
        {
            if (row.Ngaytra < row.Ngaymuon)
                throw new InvalidOperationException("Ngày trả không được trước ngày mượn.");

            Xe item = session.Find<Xe>(vehicle => vehicle.Maxe == row.Maxe);
            if (item == null)
                throw new InvalidOperationException("Xe không còn trong danh mục.");

            var requests = session.List<YeuCau>(request => request.Maxe == row.Maxe
                && request.Ngaymuon <= row.Ngaytra && request.Ngaytra >= row.Ngaymuon)
                .Where(request => original == null || request.MaK != original.MaK || request.Ngaymuon != original.Ngaymuon)
                .Select(request => Clip(request, row.Ngaymuon, row.Ngaytra))
                .Concat(new[] { row });

            if (PeakUse(requests) > item.SoLuong)
                throw new InvalidOperationException("Không còn đủ xe trong khoảng ngày đã chọn.");
        }

        private static YeuCau Clip(YeuCau item, DateTime first, DateTime last)
        {
            return new YeuCau
            {
                Ngaymuon = item.Ngaymuon < first ? first : item.Ngaymuon,
                Ngaytra = item.Ngaytra > last ? last : item.Ngaytra
            };
        }

        private static int PeakUse(IEnumerable<YeuCau> requests)
        {
            var changes = new SortedDictionary<DateTime, int>();
            foreach (YeuCau item in requests)
            {
                AddChange(changes, item.Ngaymuon.Date, 1);
                if (item.Ngaytra.Date < DateTime.MaxValue.Date)
                    AddChange(changes, item.Ngaytra.Date.AddDays(1), -1);
            }
            int active = 0, peak = 0;
            foreach (int change in changes.Values)
            {
                active += change;
                peak = Math.Max(peak, active);
            }
            return peak;
        }

        private static void AddChange(SortedDictionary<DateTime, int> changes, DateTime day, int amount)
        {
            changes.TryGetValue(day, out int value);
            changes[day] = value + amount;
        }
    }
}
