namespace MotoGuild_API.Helpers
{
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty; // czemu nie null?
        public DateTime Created { get; set; } = DateTime.Now; //Błąd, ustawienie daty jest zależne od lokalnych ustawień serwera. To trzeba poprawić np. na polska datę zawsze albo UTC
        public DateTime Expires { get; set; } //Jest to typu DateTime więc jak ktos zainicjuje klase RefreshToken i nie ustawi Expires to bedzie to 0000-01-01. Chyba nie tego chcemy?
    }
}
