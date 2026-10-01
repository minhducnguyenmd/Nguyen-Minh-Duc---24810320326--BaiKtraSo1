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

##Câu 3: Phân biệt sự khác nhau giữa phương thức virtual ở lớp cha và phương thức override ở lớp con khi triển khai tính Đa hình (Polymorphism)

- Virtual (ở lớp cha):

+ Khai báo một phương thức kèm theo hành vi mặc định.

+ Từ khóa virtual đóng vai trò cấp quyền (cho phép) cho các lớp con được phép ghi đè/thay đổi hành vi của phương thức này nếu cần.

- Override (ở lớp con):

+ Khai báo ở lớp con để thực sự ghi đè (định nghĩa lại) hành vi của phương thức virtual nhận từ lớp cha.

+ Khi gọi phương thức qua một tham chiếu kiểu lớp cha nhưng trỏ đến đối tượng lớp con, phiên bản override ở lớp con sẽ được ưu tiên thi hành (Dynamic Dispatch / Late Binding).

```C#
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

##Câu 4: Tại sao một thành phần được khai báo là static trong Lớp (Class) lại không thể truy xuất thông qua một thể hiện (Object Instance) được tạo bằng toán tử new?
Thành phần static (biến, phương thức, thuộc tính) thuộc về bản thân Lớp (Class level) chứ không thuộc về thể hiện cụ thể nào của lớp (Instance level).

- Về cơ chế bộ nhớ: Thành phần static được khởi tạo và lưu trữ tại một vùng nhớ dùng chung duy nhất (Static Heap) ngay khi Lớp được nạp vào bộ nhớ. Tất cả các đối tượng tạo bằng new đều chia sẻ chung thành phần này, nó không nằm bên trong vùng nhớ của từng thể hiện riêng biệt.

- Về quy tắc thiết kế C#: Ngôn ngữ C# quy định bắt buộc truy xuất thành phần static qua tên lớp (Ví dụ: ClassName.StaticMethod()) nhằm:

+ Tránh gây nhầm lẫn về phạm vi dữ liệu (giúp lập trình viên nhận biết rõ đây là hành vi/dữ liệu dùng chung, không phụ thuộc vào trạng thái riêng của đối tượng).

+ Tăng tính tường minh cho mã nguồn và hỗ trợ trình biên dịch tối ưu hóa mã lệnh.
