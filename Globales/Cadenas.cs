namespace Globales
{
    public class Cadenas
    {
        private const string connectionString = "Data Source=REPH-NITRO-5;Initial Catalog=InformacionAPF;Persist Security Info=True;User ID=qc2;Password=1nt3rm3x.";
        private const string regEditID = "Peta Computing";
        private const string regKeyEstado = "ETAT";
        private const string estadoActivado = "Activado";
        private const string estadoDesactivado = "Desactivado";
        private const string regKeyFinService = "HA";    /* Hasta Aqui - fin de servicio debe ser segundos epoc, 0 = por siempre */

        public string ConnectionString()
        {
            return connectionString;
        }

        public string RegEditID()
        {
            return regEditID;
        }

        public string RegKeyEstado()
        {
            return regKeyEstado;
        }

        public string EstadoActivado()
        {
            return estadoActivado;
        }

        public string EstadoDesactivado()
        {
            return estadoDesactivado;
        }

        public string RegKeyFinService()
        {
            return regKeyFinService;
        }
    }
}
