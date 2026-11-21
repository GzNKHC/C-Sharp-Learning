using System;
using System.Linq;
using System.Windows.Forms;
using ThucHanh9.Models;

namespace ThucHanh9
{
    internal static class OrderForms
    {
        private static readonly Field PartnerKey = TableInfo.Fields(typeof(DoiTac)).Single(field => field.IsKey);
        private static readonly Field PartnerName = TableInfo.Fields(typeof(DoiTac))[1];
        private static readonly string PartnerColumn = PartnerKey.Name;
        private static readonly Field OrderPartner = TableInfo.Find(typeof(DonHang), PartnerColumn);
        private static readonly Field ItemKey = TableInfo.Fields(typeof(MatHang)).Single(field => field.IsKey);

        public static CatalogOptions<DonHang> Options(bool manage)
        {
            return new CatalogOptions<DonHang>
            {
                Navigate = manage, Add = !manage, Edit = false, Delete = manage,
                Defaults = () => new DonHang { NgayLap = DateTime.Today, NgayNhanHang = DateTime.Today.AddDays(1) },
                Locked = new[] { "NgayLap" },
                Validate = (session, row, previous) => ValidateOrder(row)
            };
        }

        private static void ValidateOrder(DonHang order)
        {
            order.NgayLap = DateTime.Today;
            if (order.NgayNhanHang <= order.NgayLap)
                throw new InvalidOperationException("Ngày nhận hàng phải sau ngày lập đơn hàng.");
        }

        public static Form ForPartner(DataStore store, string title, bool cancelOrders)
        {
            var form = new RelationForm<DonHang, ChiTietDonHang>(store, title, "Đơn hàng", "Chi tiết đơn hàng",
                (session, order) => session.List<ChiTietDonHang>(row => row.MaDonHang == order.MaDonHang));
            ComboBox person = Ui.Combo(form.ControlsBar, PartnerKey.Header, 235);
            TextBox name = Ui.TextBox(form.ControlsBar, PartnerName.Header, 230);
            TextBox phone = Ui.TextBox(form.ControlsBar, "Điện thoại", 130);
            TextBox address = Ui.TextBox(form.ControlsBar, "Địa chỉ", 220);
            name.ReadOnly = phone.ReadOnly = address.ReadOnly = true;
            if (cancelOrders)
                form.Parents.MultiSelect = true;

            Action<string> refresh = select =>
            {
                string code = Ui.Code(person);
                if (code.Length != 0)
                {
                    var filter = QueryFilter.For<DonHang>(new[] { new Criterion(PartnerColumn, code, true) });
                    form.ReloadParents(filter, select);
                }
                else
                {
                    Ui.Bind(form.Parents, new System.Collections.Generic.List<DonHang>());
                    form.ReloadChildren();
                }
            };

            person.SelectedIndexChanged += (sender, args) => Ui.Run(form, () =>
            {
                string code = Ui.Code(person);
                var criteria = QueryFilter.For<DoiTac>(new[] { new Criterion(PartnerColumn, code, true) });
                DoiTac row = code.Length == 0 ? null : store.Read(s => s.Find(criteria));
                name.Text = row == null ? "" : PartnerName.Format(PartnerName.Get(row));
                phone.Text = row?.SoDienThoai ?? "";
                address.Text = row?.DiaChi ?? "";
                refresh(null);
            });

            if (cancelOrders)
            {
                Ui.Button(form.Actions, "Xóa đơn hàng", () =>
                {
                    string code = Ui.Require(Ui.Code(person), PartnerKey.Header.ToLower());
                    DonHang[] selected = form.Parents.SelectedRows.Cast<DataGridViewRow>()
                        .Select(row => row.DataBoundItem as DonHang).Where(row => row != null).ToArray();
                    if (selected.Length == 0)
                        throw new InvalidOperationException("Hãy chọn một hoặc nhiều đơn hàng.");
                    if (Ui.Ask(form, "Xóa " + selected.Length + " đơn hàng và các chi tiết tương ứng?"))
                    {
                        store.Write(s =>
                        {
                            foreach (DonHang order in selected)
                            {
                                DonHang current = s.Find<DonHang>(row => row.MaDonHang == order.MaDonHang);
                                if (current == null || !Equals(OrderPartner.Get(current), code))
                                    throw new InvalidOperationException("Đơn hàng đã thay đổi. Hãy chọn lại.");
                                s.Delete(current);
                            }
                        });
                        refresh(null);
                    }
                }, true);
            }
            else
            {
                Ui.Button(form.Actions, "Nhập chi tiết đơn hàng", () =>
                {
                    DonHang order = form.ParentRow ?? throw new InvalidOperationException("Hãy chọn một đơn hàng.");
                    if (AddDetail(form, store, order))
                        form.ReloadChildren();
                }, true);
            }

            Ui.Button(form.Actions, "Thoát", form.Close);
            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                var choices = store.Read(s => s.List<DoiTac>()).OrderBy(TableInfo.KeyText)
                    .Select(row => new Choice(PartnerKey.Get(row), TableInfo.KeyText(row) + " – " + PartnerName.Format(PartnerName.Get(row))));
                Ui.Choices(person, choices, first: false);
            });
            return form;
        }

        public static Form Details(DataStore store, string title)
        {
            var form = new RelationForm<DonHang, ChiTietDonHang>(store, title, "Đơn hàng", "Chi tiết đơn hàng",
                (s, header) => s.List<ChiTietDonHang>(row => row.MaDonHang == header.MaDonHang));
            ComboBox order = Ui.Combo(form.ControlsBar, "Mã đơn hàng", 150);
            order.SelectedIndexChanged += (sender, args) =>
            {
                string code = Ui.Code(order);
                if (code.Length > 0)
                    Ui.SelectRow<DonHang>(form.Parents, row => row.MaDonHang == code);
            };
            form.SelectedParentChanged = row =>
            {
                if (row != null && Ui.Code(order) != row.MaDonHang)
                    Ui.Select(order, row.MaDonHang);
            };
            Ui.Button(form.Actions, "Thêm", () =>
            {
                DonHang selected = form.ParentRow ?? throw new InvalidOperationException("Hãy chọn một đơn hàng.");
                if (AddDetail(form, store, selected))
                    form.ReloadChildren();
            }, true);
            Ui.Button(form.Actions, "Thoát", form.Close);
            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                form.ReloadParents();
                form.BindParentCombo(order);
            });
            return form;
        }

        private static bool AddDetail(Form form, DataStore store, DonHang order)
        {
            var defaults = new ChiTietDonHang { MaDonHang = order.MaDonHang, SoLuong = 1 };
            Field[] fields = TableInfo.Fields(typeof(ChiTietDonHang));
            Field itemCode = fields.Single(field => field.IsKey && field.Name != "MaDonHang");
            Field unit = fields.SingleOrDefault(field => field.Name == "DonViTinh");

            return EditDialog.Show(form, "Thêm chi tiết cho " + order.MaDonHang, store, defaults, detail =>
                store.Write(s =>
                {
                    if (s.Find<DonHang>(row => row.MaDonHang == order.MaDonHang) == null)
                        throw new InvalidOperationException("Đơn hàng không còn tồn tại.");

                    var criteria = QueryFilter.For<MatHang>(new[] { new Criterion(ItemKey.Name, itemCode.Get(detail), true) });
                    MatHang product = s.Find(criteria);
                    if (product == null)
                        throw new InvalidOperationException("Mặt hàng không còn tồn tại.");
                    if (unit != null)
                        unit.Set(detail, product.DonViTinh);
                    s.Insert(detail);
                }), hidden: unit == null ? null : new[] { unit.Name }, locked: new[] { "MaDonHang" });
        }

        public static Form CustomerOrders(DataStore store)
        {
            var form = new RelationForm<DoiTac, DonHang>(store, "Lập đơn hàng cho khách", "Khách hàng", "Đơn hàng",
                (session, customer) => session.List(QueryFilter.For<DonHang>(new[]
                {
                    new Criterion(PartnerColumn, PartnerKey.Get(customer), true)
                })));
            Ui.Button(form.Actions, "Thêm khách", () =>
            {
                string code = null;
                if (EditDialog.Show(form, "Thêm khách hàng", store, new DoiTac(), row =>
                    store.Write(s =>
                    {
                        code = s.NextCode("KH");
                        PartnerKey.Set(row, code);
                        s.Insert(row);
                    }), hidden: new[] { PartnerColumn }))
                    form.ReloadParents(selectedKey: code);
            }, true);
            Ui.Button(form.Actions, "Lập đơn hàng", () =>
            {
                DoiTac partner = form.ParentRow ?? throw new InvalidOperationException("Hãy chọn khách hàng.");
                var defaults = new DonHang { NgayLap = DateTime.Today, NgayNhanHang = DateTime.Today.AddDays(1) };
                OrderPartner.Set(defaults, PartnerKey.Get(partner));
                if (EditDialog.Show(form, "Lập đơn hàng", store, defaults, order =>
                {
                    ValidateOrder(order);
                    store.Write(s => s.Insert(order));
                }, locked: new[] { "NgayLap", PartnerColumn }))
                    form.ReloadChildren();
            });
            Ui.Button(form.Actions, "Thoát", form.Close);
            form.Load += (sender, args) => Ui.Run(form, () => form.ReloadParents());
            return form;
        }
    }
}
