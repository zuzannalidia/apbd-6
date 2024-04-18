using RestApiApbdPjatkCw4.Models;
using RestApiApbdPjatkCw4.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace RestApiApbdPjatkCw4.Repository
{
    public interface IAnimalRepository
    {
        Task<bool> Exist(int id);
        Task Create(Animal animal);
        Task<List<Animal>> GetAll(string orderBy);
        Task<bool> Update(string id, UpdateAnimal animal);
        Task<bool> Delete(string id);
    }

    /* Klasa implementująca nasz interfejs*/
    public class AnimalRepository: IAnimalRepository
    {
        /*Dostęp do konfiguracji*/
        private readonly IConfiguration _configration;

        public AnimalRepository(IConfiguration configuration)
        {
            _configration = configuration;
        }

        /* Przeniesione z AnimalsController */
        public async Task<List<Animal>> GetAll(string orderBy)
        {
            /* Lista zwierząt z modelu */
            var animals = new List<Animal>();

            /* Sterownik - do porozumienia się z bazą danych */
            /* Podajemy pod jakim kluczem jest zapisany ConnectionString */
            await using (var connection = new SqlConnection(_configration.GetConnectionString("Default")))
            {
                /* Stworzenie komendy */
                var command = connection.CreateCommand();

                /* Komenda do wykonywania poleceń */
                /* Sortowanie jest zawsze w kierunku „ascending” */
                command.CommandText = $"select * from animal order by {orderBy} asc";
                /* Otwarcie połączenia z bazą danych */
                await connection.OpenAsync();
                /* Wykonanie zapytania -> dostajemy wartości z BD wiec Reader */
                var reader = await command.ExecuteReaderAsync();

                /* Dopóki nasz Reader jest w stanie coś przeczytać */
                while (await reader.ReadAsync())
                {
                    animals.Add(new Animal
                    {
                        /*Int32 bo wiemy że chcemy przechwycić wartość w postaci int*/
                        /*Cyfry w nawiasach od nr. kolumny*/
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Category = reader.GetString(3),
                        Area = reader.GetString(4)
                    });
                }
            }

            return animals;
        }

        public async Task<bool> Exist(int id)
        {
            /* Sterownik - do porozumienia się z bazą danych */
            /* Podajemy pod jakim kluczem jest zapisany ConnectionString */
            await using (var connection = new SqlConnection(_configration.GetConnectionString("Default")))
            {
                /* Stworzenie komendy */
                var command = connection.CreateCommand();

                /* Komenda do wykonywania poleceń */
                command.CommandText = "select * from Animal where ID = @1";

                /* Dodawanie sprawdzenia czy pod tą wartością występuje jakaś wartość*/
                command.Parameters.AddWithValue("@1", id);

                /* Otwarcie połączenia z bazą danych */
                await connection.OpenAsync();
                
                /* Scalar zwróci wartość z 1 kolumny naszego zbioru wynikowego */
                /*Sprawdzenie czy ID istnieje*/
                if (await command.ExecuteScalarAsync() is not null)
                {
                    /*Należy sprawdzić czy wybrane ID jest unikalne. Jeśli nie trzeba zwrócić kod błędu HTTP 409*/
                    return true;
                }

                return false;
            }
        }

        public async Task Create(Animal animal)
        {
            /* Sterownik - do porozumienia się z bazą danych */
            /* Podajemy pod jakim kluczem jest zapisany ConnectionString */
            await using (var connection = new SqlConnection(_configration.GetConnectionString("Default")))
            {
                /* Stworzenie komendy */
                var command = connection.CreateCommand();

                /* Komenda do wykonywania poleceń */
                /* Sortowanie jest zawsze w kierunku „ascending” */
                command.CommandText = "insert into Animal (ID, Name, Description, Category, Area) values (@1,@2,@3,@4,@5)";

                /* Dodawanie sprawdzenia czy pod tą wartością występuje jakaś wartość*/
                command.Parameters.AddWithValue("@1", animal.ID);
                command.Parameters.AddWithValue("@2", animal.Name);
                command.Parameters.AddWithValue("@3", animal.Description);
                command.Parameters.AddWithValue("@4", animal.Category);
                command.Parameters.AddWithValue("@5", animal.Area);

                /* Otwarcie połączenia z bazą danych */
                await connection.OpenAsync();
                
                /* Wykonanie polecenia i czekanie na odpowiedź */
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> Update(string id, UpdateAnimal animal)
        {
            /* Sterownik - do porozumienia się z bazą danych */
            /* Podajemy pod jakim kluczem jest zapisany ConnectionString */
            await using (var connection = new SqlConnection(_configration.GetConnectionString("Default")))
            {
                /* Stworzenie komendy */
                var command = connection.CreateCommand();

                /* Komenda do wykonywania poleceń */
                /* Sortowanie jest zawsze w kierunku „ascending” */
                command.CommandText = $"update Animal set Name = @2, Description = @3, Category = @4, Area = @5 where ID = {id}";

                /* Dodawanie sprawdzenia czy pod tą wartością występuje jakaś wartość*/
                command.Parameters.AddWithValue("@2", animal.Name);
                command.Parameters.AddWithValue("@3", animal.Description);
                command.Parameters.AddWithValue("@4", animal.Category);
                command.Parameters.AddWithValue("@5", animal.Area);

                /* Otwarcie połączenia z bazą danych */
                await connection.OpenAsync();
                
                /* Wykonanie polecenia i czekanie na odpowiedź */
                var iloscRzedow = await command.ExecuteNonQueryAsync();

                return iloscRzedow != 0;
            }
        }

        public async Task<bool> Delete(string id)
        {
            /* Sterownik - do porozumienia się z bazą danych */
            /* Podajemy pod jakim kluczem jest zapisany ConnectionString */
            await using (var connection = new SqlConnection(_configration.GetConnectionString("Default")))
            {
                /* Stworzenie komendy */
                var command = connection.CreateCommand();

                /* Komenda do wykonywania poleceń */
                /* Sortowanie jest zawsze w kierunku „ascending” */
                command.CommandText = $"delete from Animal where ID = {id}";

                /* Otwarcie połączenia z bazą danych */
                await connection.OpenAsync();
                
                /* Wykonanie polecenia i czekanie na odpowiedź */
                var iloscRzedow = await command.ExecuteNonQueryAsync();

                return iloscRzedow != 0;
            }
        }
    }
}