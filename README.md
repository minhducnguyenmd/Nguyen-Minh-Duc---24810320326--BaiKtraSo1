# Nguyen-Minh-Duc---24810320326
## Câu 1: Trình bày sự khác nhau giữa Value Types và Reference Types (Stack vs Heap)

* **Value Types (Kiểu giá trị):** 
  * **Cơ chế:** Lưu **giá trị thực sự** trực tiếp trên **Stack**.
  * **Khi sao chép (`=`):** Sao chép toàn bộ giá trị. Thay đổi biến này **không ảnh hưởng** đến biến kia.
  * **Ví dụ:** `int`, `float`, `bool`, `struct`.

* **Reference Types (Kiểu tham chiếu):** 
  * **Cơ chế:** Đối tượng thực sự lưu trên **Heap**, còn biến trên **Stack** chỉ lưu **địa chỉ trỏ đến** Heap.
  * **Khi sao chép (`=`):** Chỉ sao chép địa chỉ. Cả 2 biến **cùng trỏ vào 1 đối tượng**, nên sửa ở biến này thì biến kia cũng bị thay đổi theo.
  * **Ví dụ:** `class`, `string`, `object`.
---

## Câu 2: Tính năng Init-only Properties (`init`) trong C# 9/10 khác gì so với `set` thông thường? Nêu trường hợp sử dụng thực tế.

* **Sự khác biệt:**
  * **`set` (thông thường):** Cho phép gán hoặc thay đổi giá trị của thuộc tính **bất kỳ lúc nào** trong suốt vòng đời của đối tượng.
  * **`init` (Init-only setter):** Chỉ cho phép gán giá trị **một lần duy nhất** tại thời điểm khởi tạo đối tượng (qua Constructor hoặc Object Initializer). Sau khi khởi tạo xong, thuộc tính trở thành Read-Only (chỉ đọc) và không thể sửa đổi từ bên ngoài.

* **Trường hợp sử dụng thực tế:**
  * Dùng khi thiết kế các đối tượng **Bất biến (Immutable Objects)** hoặc các lớp **Data Transfer Objects (DTOs)**, giúp bảo đảm dữ liệu không bị thay đổi trái phép sau khi được tạo ra nhưng vẫn giữ được cú pháp khởi tạo linh hoạt dạng Object Initializer.

```csharp
public class Person
{
    public string Id { get; init; } // Chỉ được gán khi khởi tạo
    public string Name { get; set; } // Có thể thay đổi bất kỳ lúc nào
}

// Khởi tạo đối tượng:
var p = new Person { Id = "123", Name = "Nguyen Van A" };

// p.Id = "456"; // LỖI BIÊN DỊCH: Cannot assign to property 'Id' except in an object initializer
p.Name = "Nguyen Van B"; // Hợp lệ
```

---

### Câu 3: Phân biệt `virtual` (lớp cha) và `override` (lớp con) trong Đa hình

* **`virtual` (Lớp cha):** Khai báo một phương thức kèm hành vi mặc định, đồng thời **cho phép** các lớp con được quyền thay đổi (ghi đè) hành vi đó.
* **`override` (Lớp con):** Đặt ở lớp con để **thực sự thay thế** (ghi đè) lại hành vi của phương thức `virtual` từ lớp cha.
> 
```
public class Animal
{
    public virtual void Speak() => Console.WriteLine("Animal sound");
}

public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Woof!");
}
```
---

### Câu 4: Tại sao không thể gọi thành phần `static` từ một đối tượng (`new`)?

* **Lý do bộ nhớ:** Thành phần `static` thuộc về **toàn bộ Lớp (Class)** chứ không thuộc về từng đối tượng riêng lẻ. Nó được tạo ra một lần duy nhất ở vùng nhớ chung ngay khi chương trình chạy.
* **Lý do thiết kế:** C# bắt buộc gọi qua tên lớp (ví dụ: `ClassName.Method()`) để code rõ ràng, tránh nhầm lẫn giữa **dữ liệu dùng chung** của Class và **dữ liệu riêng** của từng đối tượng.
