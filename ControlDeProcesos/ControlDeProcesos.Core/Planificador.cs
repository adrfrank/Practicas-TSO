using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeProcesos.Core
{
    public class Planificador
    {
        int cont = 0;
        protected DateTime lastTime;
        protected DateTime lastEjecution;
        protected TimeSpan TiempoEjecucion;
        protected int maxListo;


        public List<Proceso> TablaProcesos { get; set; }
        public Queue<Proceso> Nuevo { get; set; }
        public List<Proceso> Listo { get; set; }
        public Queue<Proceso> Ejecucion { get; set; }
        public Queue<Proceso> Terminado { get; set; }
        public Queue<Proceso> Bloqueado { get; set; }

        public IFabricaProceso Fabrica { get; set; }


        public int MaxListo { get {
                return maxListo - Bloqueado.Count;
            } set {
                maxListo = value;
            } }
       
        public bool Iniciado { get; set; }

        public event EventHandler ProcesosTerminados;

        public Planificador()
        {
            Nuevo = new Queue<Proceso>();
            Listo = new List<Proceso>();
            Ejecucion = new Queue<Proceso>();
            Terminado = new Queue<Proceso>();
            Bloqueado = new Queue<Proceso>();
            TablaProcesos = new List<Proceso>();
            MaxListo = 5;
            lastTime = DateTime.Now;
            lastEjecution = DateTime.MinValue;

        }

        public virtual void NextTime(DateTime time)
        {
            if (Iniciado)
            {

                if (Ejecucion.FirstOrDefault() == null)
                {
                    if (Listo.Count == MaxListo || Listo.Count > 0 && Nuevo.Count == 0)
                    {
                        Ejecucion.Enqueue(Listo.First());
                        Listo.Remove(Listo.First());                        
                    }
                    if (Listo.Count < MaxListo && Nuevo.Count > 0)                       
                        Listo.Add(Nuevo.Dequeue());
                }
                else
                {
                    Proceso ejec = Ejecucion.First();
                    ejec.TiempoProcesado += time - lastTime;
                    if (ejec.TiempoRespuesta == null) ejec.TiempoRespuesta = TiempoEjecucion;
                    TiempoEjecucion += time - lastTime;

                    if (ejec.TiempoProcesado >= ejec.TiempoDeServicio)
                    {
                        Terminado.Enqueue(Ejecucion.Dequeue());
                        if (Listo.Count > 0)
                        {
                            Ejecucion.Enqueue(Listo.First());
                            Listo.Remove(Listo.First());

                            if (Listo.Count < MaxListo && Nuevo.Count > 0)
                            {
                                Listo.Add(Nuevo.Dequeue());
                            }


                        }
                    }

                }
                //Todos los procesos terminados
                if (Ejecucion.Count == 0 && Listo.Count == 0 && Nuevo.Count == 0)
                {
                    Iniciado = false;
                    OnProcesosTerminados();
                }

                
            }
            lastTime = time;
        }
      

        public virtual void Iniciar()
        {
            Iniciado = true;
            TiempoEjecucion = TimeSpan.Zero;
        }

        public virtual void Detener()
        {
            Iniciado = false;
            while (Ejecucion.Count > 0) {
                DetenerProceso();
            }
            while (Listo.Count > 0)
            {
                var p = Listo.First();
                p.TerminoConError = true;
                Terminado.Enqueue(p);
                Listo.Remove(Listo.First());

            }
            while (Nuevo.Count > 0)
            {
                var p = Nuevo.Dequeue();
                p.TerminoConError = true;
                Terminado.Enqueue(p);
            }
        }

        public virtual void DetenerProceso()
        {
            if (Ejecucion.Count > 0) {
                var p = Ejecucion.Dequeue();
                p.TerminoConError = true;
                Terminado.Enqueue(p);                
            }
        }

        public virtual void BloquearProceso() {
            Bloqueado.Enqueue(Ejecucion.Dequeue());
            if (Listo.Count > 0)
            {
                Ejecucion.Enqueue(Listo.First());
                Listo.Remove(Listo.First());
            }
        }

        protected virtual void OnProcesosTerminados() {
            if (ProcesosTerminados != null)
                ProcesosTerminados(this, new EventArgs());
        }

        public void GenerarProcesos(int num)
        {
            Random rdn = new Random();
            Nuevo.Clear();
            for (var i = 0; i < num; ++i)
            {
                var p = Fabrica.CrearProceso(cont++, 0, rdn.Next(5), rdn.Next(5) + 6);
                Nuevo.Enqueue(p);
                TablaProcesos.Add(p);
            }
        }
    }
}
