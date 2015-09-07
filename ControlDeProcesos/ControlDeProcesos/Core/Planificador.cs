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
        DateTime lastTime;
        DateTime lastEjecution;

        public Queue<Proceso> Nuevo { get; set; }
        public Queue<Proceso> Listo { get; set; }
        public Queue<Proceso> Ejecucion { get; set; }
        public Queue<Proceso> Terminado { get; set; }
        public Queue<Proceso> Bloqueado { get; set; }

        public IFabricaProceso Fabrica { get; set; }

        public int MaxListo { get; set; }
       
        public bool Iniciado { get; set; }

        public event EventHandler ProcesosTerminados;

        public Planificador()
        {
            Nuevo = new Queue<Proceso>();
            Listo = new Queue<Proceso>();
            Ejecucion = new Queue<Proceso>();
            Terminado = new Queue<Proceso>();
            Bloqueado = new Queue<Proceso>();
            MaxListo = 5;
            lastTime = DateTime.Now;
            lastEjecution = DateTime.MinValue;
        }

        public void NextTime(DateTime time)
        {
            if (Iniciado)
            {

                if (Ejecucion.FirstOrDefault() == null)
                {
                    if (Listo.Count == MaxListo || Listo.Count > 0 && Nuevo.Count == 0)
                    {
                        Ejecucion.Enqueue(Listo.Dequeue());
                        if (Nuevo.Count > 0)
                            Listo.Enqueue(Nuevo.Dequeue());
                    }
                    else
                    {
                        Listo.Enqueue(Nuevo.Dequeue());
                    }
                }
                else
                {
                    Proceso ejec = Ejecucion.First();
                    ejec.TiempoProcesado += time - lastTime;
                    if (ejec.TiempoProcesado >= ejec.Time)
                    {
                        Terminado.Enqueue(Ejecucion.Dequeue());
                        if (Listo.Count > 0)
                        {
                            Ejecucion.Enqueue(Listo.Dequeue());
                            if(Listo.Count < MaxListo && Nuevo.Count > 0)
                                Listo.Enqueue(Nuevo.Dequeue());


                        }
                    }

                }
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
        }

        public virtual void Detener()
        {
            Iniciado = false;
            while (Ejecucion.Count > 0) {
                Terminado.Enqueue(Ejecucion.Dequeue());
            }
            while (Listo.Count > 0)
            {
                Terminado.Enqueue(Listo.Dequeue());
            }
            while (Nuevo.Count > 0)
            {
                Terminado.Enqueue(Nuevo.Dequeue());
            }
        }

        public virtual void DetenerProceso()
        {
            if (Ejecucion.Count > 0)
                Terminado.Enqueue(Ejecucion.Dequeue());
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
                var p = Fabrica.CrearProceso(cont++, 0, rdn.Next(5), rdn.Next(5) + 1);
                Nuevo.Enqueue(p);
            }
        }
    }
}
