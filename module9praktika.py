from abc import ABC, abstractmethod

class RoomBookingSystem:
    def book_room(self, guest_name):
        print(f"Бронирование номера для гостя: {guest_name}")

    def cancel_booking(self, guest_name):
        print(f"Отмена бронирования для гостя: {guest_name}")


class RestaurantSystem:
    def book_table(self, guest_name):
        print(f"Стол забронирован для гостя: {guest_name}")

    def order_food(self, guest_name, dishes):
        print(f"Заказ еды для {guest_name}: {', '.join(dishes)}")


class EventManagementSystem:
    def organize_event(self, event_name):
        print(f"Организовано мероприятие: {event_name}")

    def book_equipment(self, equipment_list):
        print(f"Забронировано оборудование: {', '.join(equipment_list)}")


class CleaningService:
    def schedule_cleaning(self, room_number):
        print(f"Уборка запланирована для комнаты №{room_number}")

    def clean_now(self, room_number):
        print(f"Уборка выполнена в комнате №{room_number}")


class TaxiService:
    def order_taxi(self, guest_name):
        print(f"Такси вызвано для гостя: {guest_name}")


# ФАСАД
class HotelFacade:
    def __init__(self):
        self.booking = RoomBookingSystem()
        self.restaurant = RestaurantSystem()
        self.events = EventManagementSystem()
        self.cleaning = CleaningService()
        self.taxi = TaxiService()

    def book_room_with_services(self, guest_name, dishes):
        print("\n Сценарий: Бронирование номера с едой и уборкой ")
        self.booking.book_room(guest_name)
        self.restaurant.order_food(guest_name, dishes)
        self.cleaning.schedule_cleaning(101)
        print("Все услуги успешно оформлены")

    def organize_event_with_rooms(self, event_name, participants, equipment):
        print("\n Сценарий: Организация мероприятия ")
        self.events.organize_event(event_name)
        self.events.book_equipment(equipment)
        for p in participants:
            self.booking.book_room(p)
        print("Мероприятие и бронирования участников оформлены")

    def book_table_with_taxi(self, guest_name):
        print("\n Сценарий: Бронирование стола с вызовом такси ")
        self.restaurant.book_table(guest_name)
        self.taxi.order_taxi(guest_name)
        print("Стол и такси оформлены")

    def cancel_booking(self, guest_name):
        print("\n Отмена бронирования ")
        self.booking.cancel_booking(guest_name)

    def clean_on_request(self, room_number):
        print("\n Уборка по запросу ")
        self.cleaning.clean_now(room_number)

class OrganizationComponent(ABC):
    @abstractmethod
    def display(self, indent=0):
        pass

    @abstractmethod
    def get_budget(self):
        pass

    @abstractmethod
    def get_employee_count(self):
        pass


class Employee(OrganizationComponent):
    def __init__(self, name, position, salary):
        self.name = name
        self.position = position
        self.salary = salary

    def display(self, indent=0):
        print(" " * indent + f"{self.position}: {self.name} (Зарплата: {self.salary})")

    def get_budget(self):
        return self.salary

    def get_employee_count(self):
        return 1

    def set_salary(self, new_salary):
        self.salary = new_salary
        print(f"Зарплата сотрудника {self.name} изменена на {new_salary}")


class Contractor(Employee):
    def get_budget(self):
        return 0  


class Department(OrganizationComponent):
    def __init__(self, name):
        self.name = name
        self.children = []

    def add(self, component):
        if component not in self.children:
            self.children.append(component)

    def remove(self, component):
        if component in self.children:
            self.children.remove(component)

    def display(self, indent=0):
        print(" " * indent + f"[Отдел] {self.name}")
        for child in self.children:
            child.display(indent + 4)

    def get_budget(self):
        return sum(child.get_budget() for child in self.children)

    def get_employee_count(self):
        return sum(child.get_employee_count() for child in self.children)

    def find_employee(self, name):
        for child in self.children:
            if isinstance(child, Employee) and child.name == name:
                return child
            elif isinstance(child, Department):
                found = child.find_employee(name)
                if found:
                    return found
        return None


if __name__ == "__main__":
    # --- ФАСАД ---
    hotel = HotelFacade()
    hotel.book_room_with_services("Иван Кличко", ["Пицца", "Кофе"])
    hotel.organize_event_with_rooms("IT Конференция", ["Анна", "Борян"], ["Проектор", "Микрофон"])
    hotel.book_table_with_taxi("Сергей")
    hotel.clean_on_request(203)
    hotel.cancel_booking("Иван Кличко")

    # --- КОМПОНОВЩИК ---
    print("\n================================")
    print("    КОРПОРАТИВНАЯ СТРУКТУРА")
    print("================================")

    e1 = Employee("Иван Кличко", "Менеджер", 120000)
    e2 = Employee("Анна Хилькевич", "Разработчик", 150000)
    e3 = Contractor("Дмитрий Нагиев", "Тестировщик (контрактор)", 70000)
    e4 = Employee("Мария Абрамович", "HR", 90000)

    dev_dept = Department("Отдел Разработки")
    hr_dept = Department("Отдел HR")
    main_office = Department("Главный офис")

    dev_dept.add(e2)
    dev_dept.add(e3)
    hr_dept.add(e4)
    main_office.add(e1)
    main_office.add(dev_dept)
    main_office.add(hr_dept)

    print("\n Структура организации ")
    main_office.display()
    print(f"\nОбщий бюджет: {main_office.get_budget()} руб.")
    print(f"Всего сотрудников: {main_office.get_employee_count()}")

    print("\n Поиск сотрудника ")
    found = main_office.find_employee("Анна Хилькевич")
    if found:
        found.display()
    else:
        print("Сотрудник не найден")
