using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeProcesos.Core
{
    public class Planificador
    {
        public List<Proceso> Nuevo { get; set; }
        public List<Proceso> Listo { get; set; }
        public List<Proceso> Ejecucion { get; set; }
        public List<Proceso> Terminado { get; set; }
        public List<Proceso> Bloqueado { get; set; }

        public IFabricaProceso Fabrica { get; set; }

        int cont = 0;

        public bool Iniciado { get; set; }

        public Planificador()
        {
            Nuevo = new List<Proceso>();
            Listo = new List<Proceso>();
            Ejecucion = new List<Proceso>();
            Terminado = new List<Proceso>();
            Bloqueado = new List<Proceso>();
        }

        public virtual void Iniciar() {
            Iniciado = true;
        }

        public virtual void Detener() {
            Iniciado = false;
        }

        public virtual void DetenerProceso() {

        }

        public void GenerarProcesos(int num) {
            Random rdn = new Random();
            Nuevo.Clear();
            for (var i = 0; i < num; ++i) {
                var p = Fabrica.CrearProceso(cont++, 0, rdn.Next(5), rdn.Next(5));
                Nuevo.Add(p);
            }
        }
    }
}
