namespace ConsoleAppd_pHw2102Bus;

class Program
{
    static int passengersAtStop = 0; // количество пассажиров на остановке
    static int busCapacity = 50; // вместимость автобуса
    static int passengersInBus = 0; // количество пассажиров в автобусе
    static bool busArrived = false; // признак прибытия автобуса
    static AutoResetEvent busArrivalEvent = new AutoResetEvent(false); // событие прибытия автобуса
    static AutoResetEvent busDepartureEvent = new AutoResetEvent(false); // событие отправления автобуса
    static void Main()
    {
        Thread busThread = new Thread(BusThread);
        busThread.Start();

        Random rnd = new Random();

        // имитация работы остановки в течение дня
        for (int i = 0; i < 24; i++)
        {
            int passengers = rnd.Next(1, 101); // случайное количество пассажиров
            passengersAtStop += passengers; // добавляем пассажиров на остановке

            Console.WriteLine($"На остановке прибыло {passengers} пассажиров. Всего: {passengersAtStop}");

            // Оповещаем, что автобус прибыл
            busArrived = true;
            busArrivalEvent.Set();

            // Ожидаем, когда автобус уедет
            busDepartureEvent.WaitOne();

            Console.WriteLine($"Автобус уехал. Осталось {passengersAtStop} пассажиров на остановке.");
        }

        busArrivalEvent.Dispose();
        busDepartureEvent.Dispose();
    }

    static void BusThread()
    {
        while (true)
        {
            // Ожидаем прибытия автобуса
            busArrivalEvent.WaitOne();

            // Загрузка пассажиров в автобус
            while (passengersAtStop > 0 && passengersInBus < busCapacity)
            {
                passengersInBus++;
                passengersAtStop--;
            }

            Console.WriteLine($"Автобус заполнен. В автобусе: {passengersInBus}");

            // Сообщаем об отправлении автобуса
            busDepartureEvent.Set();

            // Ожидаем нового прибытия автобуса
            busArrived = false;
        }
    }
}
