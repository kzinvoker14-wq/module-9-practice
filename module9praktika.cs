using System;
using System.Collections.Generic;

public class RoomBookingSystem
{
    public void BookRoom(string guestName)
    {
        Console.WriteLine($"Бронирование номера для гостя: {guestName}");
    }

    public void CancelBooking(string guestName)
    {
        Console.WriteLine($"Отмена бронирования для гостя: {guestName}");
    }
}

public class RestaurantSystem
{
    public void BookTable(string guestName)
    {
        Console.WriteLine($"Стол забронирован для гостя: {guestName}");
    }

    public void OrderFood(string guestName, List<string> dishes)
    {
        Console.WriteLine($"Заказ еды для {guestName}: {string.Join(", ", dishes)}");
    }
}

public class EventManagementSystem
{
    public void OrganizeEvent(string eventName)
    {
        Console.WriteLine($"Организовано мероприятие: {eventName}");
    }

    public void BookEquipment(List<string> equipmentList)
    {
        Console.WriteLine($"Забронировано оборудование: {string.Join(", ", equipmentList)}");
    }
}

public class CleaningService
{
    public void ScheduleCleaning(int roomNumber)
    {
        Console.WriteLine($"Уборка запланирована для комнаты №{roomNumber}");
    }

    public void CleanNow(int roomNumber)
    {
        Console.WriteLine($"Уборка выполнена в комнате №{roomNumber}");
    }
}

public class TaxiService
{
    public void OrderTaxi(string guestName)
    {
        Console.WriteLine($"Такси вызвано для гостя: {guestName}");
    }
}

public class HotelFacade
{
    private RoomBookingSystem booking = new RoomBookingSystem();
    private RestaurantSystem restaurant = new RestaurantSystem();
    private EventManagementSystem events = new EventManagementSystem();
    private CleaningService cleaning = new CleaningService();
    private TaxiService taxi = new TaxiService();

    public void BookRoomWithServices(string guestName, List<string> dishes)
    {
        Console.WriteLine("\n Сценарий: Бронирование номера с едой и уборкой ");
        booking.BookRoom(guestName);
        restaurant.OrderFood(guestName, dishes);
        cleaning.ScheduleCleaning(101);
        Console.WriteLine("Все услуги успешно оформлены");
    }

    public void OrganizeEventWithRooms(string eventName, List<string> participants, List<string> equipment)
    {
        Console.WriteLine("\n Сценарий: Организация мероприятия ");
        events.OrganizeEvent(eventName);
        events.BookEquipment(equipment);
        foreach (string p in participants)
        {
            booking.BookRoom(p);
        }
        Console.WriteLine("Мероприятие и бронирования участников оформлены");
    }

    public void BookTableWithTaxi(string guestName)
    {
        Console.WriteLine("\n Сценарий: Бронирование стола с вызовом такси ");
        restaurant.BookTable(guestName);
        taxi.OrderTaxi(guestName);
        Console.WriteLine("Стол и такси оформлены");
    }

    public void CancelBooking(string guestName)
    {
        Console.WriteLine("\n Отмена бронирования ");
        booking.CancelBooking(guestName);
    }

    public void CleanOnRequest(int roomNumber)
    {
        Console.WriteLine("\n Уборка по запросу ");
        cleaning.CleanNow(roomNumber);
    }
}
public abstract class OrganizationComponent
{
    public abstract void Display(int indent = 0);
    public abstract double GetBudget();
    public abstract int GetEmployeeCount();
}

public class Employee : OrganizationComponent
{
    public string Name { get; set; }
    public string Position { get; set; }
    public double Salary { get; set; }

    public Employee(string name, string position, double salary)
    {
        Name = name;
        Position = position;
        Salary = salary;
    }

    public override void Display(int indent = 0)
    {
        Console.WriteLine(new string(' ', indent) + $"{Position}: {Name} (Зарплата: {Salary})");
    }

    public override double GetBudget() => Salary;

    public override int GetEmployeeCount() => 1;

    public void SetSalary(double newSalary)
    {
        Salary = newSalary;
        Console.WriteLine($"Зарплата сотрудника {Name} изменена на {newSalary}");
    }
}

public class Contractor : Employee
{
    public Contractor(string name, string position, double salary)
        : base(name, position, salary) { }

    public override double GetBudget() => 0;

public class Department : OrganizationComponent
{
    public string Name { get; set; }
    private List<OrganizationComponent> children = new List<OrganizationComponent>();

    public Department(string name)
    {
        Name = name;
    }

    public void Add(OrganizationComponent component)
    {
        if (!children.Contains(component))
            children.Add(component);
    }

    public void Remove(OrganizationComponent component)
    {
        if (children.Contains(component))
            children.Remove(component);
    }

    public override void Display(int indent = 0)
    {
        Console.WriteLine(new string(' ', indent) + $"[Отдел] {Name}");
        foreach (var child in children)
            child.Display(indent + 4);
    }

    public override double GetBudget()
    {
        double total = 0;
        foreach (var child in children)
            total += child.GetBudget();
        return total;
    }

    public override int GetEmployeeCount()
    {
        int count = 0;
        foreach (var child in children)
            count += child.GetEmployeeCount();
        return count;
    }

    public Employee FindEmployee(string name)
    {
        foreach (var child in children)
        {
            if (child is Employee emp && emp.Name == name)
                return emp;
            else if (child is Department dept)
            {
                var found = dept.FindEmployee(name);
                if (found != null)
                    return found;
            }
        }
        return null;
    }
}

public class Program
{
    public static void Main()
    {
        HotelFacade hotel = new HotelFacade();
        hotel.BookRoomWithServices("Иван Кличко", new List<string> { "Пицца", "Кофе" });
        hotel.OrganizeEventWithRooms("IT Конференция", new List<string> { "Анна", "Борян" }, new List<string> { "Проектор", "Микрофон" });
        hotel.BookTableWithTaxi("Сергей");
        hotel.CleanOnRequest(203);
        hotel.CancelBooking("Иван Кличко");

        Console.WriteLine("    КОРПОРАТИВНАЯ СТРУКТУРА");
     
        Employee e1 = new Employee("Иван Кличко", "Менеджер", 120000);
        Employee e2 = new Employee("Анна Хилькевич", "Разработчик", 150000);
        Contractor e3 = new Contractor("Дмитрий Нагиев", "Тестировщик (контрактор)", 70000);
        Employee e4 = new Employee("Мария Абрамович", "HR", 90000);

        Department devDept = new Department("Отдел Разработки");
        Department hrDept = new Department("Отдел HR");
        Department mainOffice = new Department("Главный офис");

        devDept.Add(e2);
        devDept.Add(e3);
        hrDept.Add(e4);
        mainOffice.Add(e1);
        mainOffice.Add(devDept);
        mainOffice.Add(hrDept);

        Console.WriteLine("\n Структура организации ");
        mainOffice.Display();
        Console.WriteLine($"\nОбщий бюджет: {mainOffice.GetBudget()} руб.");
        Console.WriteLine($"Всего сотрудников: {mainOffice.GetEmployeeCount()}");

        Console.WriteLine("\n Поиск сотрудника ");
        var found = mainOffice.FindEmployee("Анна Хилькевич");
        if (found != null)
            found.Display();
        else
            Console.WriteLine("Сотрудник не найден");
    }
}
