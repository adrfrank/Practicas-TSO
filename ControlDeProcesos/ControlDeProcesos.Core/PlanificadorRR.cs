using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDeProcesos.Core
{
    public class PlanificadorRR: Planificador
    {
        protected TimeSpan Quantum = TimeSpan.FromSeconds(3);
        protected TimeSpan QuantumCounter;

        void LlenarListos() {
            while (Listo.Count < MaxListo && Nuevo.Count > 0)
                Listo.Add(Nuevo.Dequeue());
        }
        public override void NextTime(DateTime time)
        {

            //Actualiza el tiempo de los procesos bloqueados
            if (Bloqueado.Count > 0)
            {
                Bloqueado.First().TiempoBloqueado += time - lastTime;
                if (Bloqueado.First().TiempoBloqueado > Bloqueado.First().TiempoDeBloque){
                    var p = Bloqueado.Dequeue();
                    p.TiempoBloqueado = TimeSpan.Zero;
                    Listo.Add(p);
                }
                    
            }

            if (Iniciado)
            {
            
                if (Ejecucion.FirstOrDefault() == null)
                {
                    if (Listo.Count == MaxListo || Listo.Count > 0 && Nuevo.Count == 0)
                    {
                        Ejecucion.Enqueue(Listo.First());
                        Listo.Remove(Listo.First());
                    }
                    LlenarListos();
                }
                else
                {
                    Proceso ejec = Ejecucion.First();
                    ejec.TiempoProcesado += time - lastTime;
                    //Tiempo de ejecución
                    if (ejec.TiempoRespuesta == null) ejec.TiempoRespuesta = TiempoEjecucion;
                    TiempoEjecucion += time - lastTime;

                    QuantumCounter += time - lastTime;

                    if (ejec.TiempoProcesado >= ejec.TiempoDeServicio)
                    {
                        Terminado.Enqueue(Ejecucion.Dequeue());
                    }
                    //Verificar quantum
                    if (QuantumCounter >= Quantum) {
                        QuantumCounter = TimeSpan.Zero;
                        //Cambio de contexto y rotar turno
                        if (Ejecucion.FirstOrDefault() != null && Listo.Count > 0) {                           
                            Listo.Add(Ejecucion.Dequeue());
                            Ejecucion.Enqueue(Listo.First());
                            Listo.Remove(Listo.First());
                        }
                    }

                }
                //Todos los procesos terminados, enviar evento de finalización
                if (Ejecucion.Count == 0 && Listo.Count == 0 && Nuevo.Count == 0)
                {
                    Iniciado = false;
                    OnProcesosTerminados();
                }


            }
            lastTime = time;
        }
    }
}
