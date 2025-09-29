# Trang Chi Tiết Báo Giá (Quotation Detail)

## Mô tả chức năng
Trang chi tiết báo giá là một trang hiển thị đầy đủ thông tin của một báo giá cụ thể với giao diện chuyên nghiệp, bao gồm:

## Các tính năng chính

### 1. **Header Thông Tin Công Ty**
- Logo và thông tin công ty
- Số báo giá và trạng thái
- Thiết kế gradient đẹp mắt

### 2. **Thông Tin Báo Giá & Khách Hàng**
- **Thông tin báo giá**: Ngày tạo, thời gian, nhân viên tư vấn, thời hạn hiệu lực
- **Thông tin khách hàng**: Họ tên, email, điện thoại, mã khách hàng

### 3. **Thông Tin Xe**
- Hãng xe, model, phiên bản
- Màu sắc, năm sản xuất, thời gian bảo hành
- Hình ảnh minh họa

### 4. **Chi Tiết Tính Giá**
- Bảng breakdown chi tiết giá:
  - Giá xe gốc
  - Giảm giá (nếu có)
  - Phí bổ sung
  - Thuế VAT
- Hiển thị giá cuối cùng nổi bật
- Thông tin bổ sung về khuyến mãi và phí

### 5. **Điều Khoản & Điều Kiện**
- Điều kiện bán hàng (hiệu lực báo giá, đặt cọc, thời gian giao xe)
- Chính sách bảo hành chi tiết

### 6. **Phương Thức Thanh Toán**
- Hiển thị các phương thức: Tiền mặt, chuyển khoản, thẻ tín dụng, trả góp
- Icon và mô tả cho từng phương thức

### 7. **Thông Tin Liên Hệ**
- Thông tin nhân viên tư vấn
- Địa chỉ showroom, giờ làm việc

### 8. **Chức Năng Hành Động**
- **In báo giá**: Tối ưu cho in ấn
- **Xuất PDF**: Chuyển đổi sang PDF
- **Duyệt báo giá**: Thay đổi trạng thái thành "Approved"
- **Chỉnh sửa**: Chuyển đến trang edit
- **Nhân bản**: Tạo báo giá mới dựa trên báo giá hiện tại
- **Xóa báo giá**: Xóa với xác nhận

## Thiết kế & UX

### **Responsive Design**
- Tối ưu cho desktop, tablet và mobile
- Layout thích ứng theo kích thước màn hình

### **Print-friendly**
- CSS riêng cho in ấn
- Ẩn các button không cần thiết khi in
- Tối ưu font size và layout cho giấy A4

### **Animations & Effects**
- Smooth transitions
- Hover effects trên buttons và cards
- Loading animations
- Float animations cho header

### **Color Scheme**
- Gradient backgrounds cho header và price highlight
- Consistent color coding cho các loại phí
- Professional color palette

## Cấu trúc File

### **Controller**: `QuotationController.cs`
```csharp
[HttpGet]
public async Task<IActionResult> Details(int id)

[HttpPost]
public async Task<JsonResult> Approve(int id)

[HttpGet]
public async Task<IActionResult> Edit(int id)

[HttpGet]
public async Task<IActionResult> Duplicate(int id)

[HttpGet]
public async Task<IActionResult> Print(int id)
```

### **View**: `Views/Quotation/Details.cshtml`
- Responsive Bootstrap layout
- Custom CSS styling
- Print-optimized design
- JavaScript for interactions

### **ViewModel**: `QuotationDetailViewModel.cs`
- Comprehensive data model
- Price breakdown calculations
- Navigation properties

### **CSS**: `wwwroot/css/quotation-detail.css`
- Custom styling
- Print media queries
- Animations and transitions
- Responsive breakpoints

## Cách sử dụng

### **Truy cập trang Details**
1. Từ trang Quotation Index
2. Click vào "View Details" trong dropdown Actions
3. Hoặc truy cập trực tiếp: `/Quotation/Details/{id}`

### **In báo giá**
1. Click nút "In báo giá"
2. Sử dụng print dialog của browser
3. Layout tự động tối ưu cho in

### **Duyệt báo giá**
1. Click nút "Duyệt báo giá"
2. Xác nhận trong popup
3. Trạng thái sẽ cập nhật thành "Approved"

### **Chỉnh sửa báo giá**
1. Click nút "Chỉnh sửa"
2. Chuyển đến form edit với dữ liệu đã có

### **Nhân bản báo giá**
1. Click nút "Nhân bản"
2. Tạo báo giá mới với thông tin tương tự
3. Trạng thái reset về "Pending"

## Tính năng kỹ thuật

### **Performance**
- Lazy loading cho data
- Optimized CSS và JavaScript
- Minimal server calls

### **Security**
- CSRF protection cho actions
- Proper authorization checks
- Input validation

### **Accessibility**
- Semantic HTML structure
- ARIA labels
- Keyboard navigation support
- Screen reader friendly

### **SEO & Meta**
- Proper page titles
- Meta descriptions
- Structured data for quotations

## Browser Support
- Chrome 80+
- Firefox 75+
- Safari 13+
- Edge 80+

## Dependencies
- Bootstrap 5
- Bootstrap Icons
- ASP.NET Core MVC
- AutoMapper
- Entity Framework Core

## Future Enhancements
- [ ] Email báo giá trực tiếp
- [ ] Digital signature
- [ ] QR code cho tracking
- [ ] Multi-language support
- [ ] Dark mode
- [ ] Export to Excel
- [ ] Báo giá template customization