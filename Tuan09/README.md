@model ProductFormViewModel
@{ ViewData["Title"] = "Sửa sản phẩm"; }
<main class="container"><div class="page-head"><h1>Sửa sản phẩm</h1><p>Ảnh mới sẽ thay ảnh đại diện; ảnh bổ sung được thêm vào bộ ảnh hiện tại.</p></div>
<div class="panel form-panel"><form asp-action="Update" asp-route-id="@Model.Id" method="post" enctype="multipart/form-data" class="form-grid">
    <input asp-for="Id" type="hidden" /><partial name="_ProductForm" model="Model" />
    @if (Model.CurrentImageUrl is not null) { <div><strong>Ảnh đại diện hiện tại</strong><div class="gallery"><img src="@Model.CurrentImageUrl" alt="Ảnh đại diện hiện tại" /></div></div> }
    @if (Model.CurrentImageUrls.Count > 0) { <div><strong>Ảnh bổ sung hiện tại</strong><div class="gallery">@foreach (var img in Model.CurrentImageUrls) { <img src="@img" alt="Ảnh bổ sung hiện tại" /> }</div></div> }
    <div class="detail-actions"><button class="button" type="submit">Lưu thay đổi</button><a asp-action="Index" class="button button-outline">Hủy</a></div>
</form></div></main>
