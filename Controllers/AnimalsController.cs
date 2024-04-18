using RestApiApbdPjatkCw4.Models;
using RestApiApbdPjatkCw4.Models.DTOs;
using RestApiApbdPjatkCw4.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace RestApiApbdPjatkCw4.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        /* Usługa przechowująca informacje o danych zapisany w appsettings.json */
        private readonly IConfiguration _configuration;

        /* Pobranie konfiguracji */
        private readonly IAnimalRepository _animalRepository;
        
        /* Przypisanie konfiguracji do kontrolera */
        public AnimalsController(IConfiguration configuration, IAnimalRepository repository)
        {
            _animalRepository = repository;
            _configuration = configuration;
        }

        /* Dodaj metodę/endpoint pozwalającą na uzyskanie listy zwierząt.
         Końcówka powinna reagować na żądanie typu HTTP GET wysłane na adres /api/animals */
        [HttpGet]

        /* Deklaracja parametru orderBy - nullowalne bo nie zawsze musimy to podać */
        public async Task<IActionResult> GetAnimals(string? orderBy = "name")
        {

            /* Jeżeli podamy coś w orderBy co nie powinno się tam znajdować - ma nam defaultuować do sortowania po name */
            switch (orderBy)
            {
                /*Parametr jako dostępne wartości przyjmuje: name, description, category, area.
                 Możemy sortować wyłącznie po jednej kolumnie.*/
                case "name": break;
                case "description": break;
                case "category": break;
                case "area": break;
                default:
                    orderBy = "name";
                    break;
            }

            var animals = await _animalRepository.GetAll(orderBy);

            /*W ramach zwrotki OK - wyświetlić listę Animals*/
            return Ok(animals);
        }


        [HttpPost]
        public async Task<IActionResult> AddAnimal(AddAnimal newAnimal)
        {
            /*SPRAWDZENIE CZY WYSTĘPUJE COŚ O TAKIM ID*/
            
            /*Sprawdzenie używając metody z repozytorium!*/
            if (await _animalRepository.Exist(newAnimal.ID))
            {
                return Conflict();
            }

            await _animalRepository.Create(new Animal
            {
                ID = newAnimal.ID,
                Area = newAnimal.Area,
                Category = newAnimal.Category,
                Description = newAnimal.Description,
                Name = newAnimal.Name
            });
            
            /* Metoda powinna odpowiadać na żądanie HTTP POST na adres api/animals */
            return Created($"/api/animals/{newAnimal.ID}", newAnimal);
        }

        [HttpPut ("id:int")]

        public async Task<IActionResult> UpdateAnimal(string id, UpdateAnimal animal)
        {
            if (await _animalRepository.Update(id,animal))
            {
                /*W przypadku sukcesu, końcówka powinna zwrócić aktualne dane aktualizowanego zwierzęcia*/
                return Ok(animal);
            }
            return NotFound();
        }

        [HttpDelete ("id:int")]
        public async Task<IActionResult> DeleteAnimal(string id)
        {
            if (await _animalRepository.Delete(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}