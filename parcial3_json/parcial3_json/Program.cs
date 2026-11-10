using Newtonsoft.Json;

var persona = new
{
    Nombre = "Cristell",
    Edad = 20,
    Carrera = "Ingeniería en Sistemas"
};

string json = JsonConvert.SerializeObject(persona);
Console.WriteLine(json);