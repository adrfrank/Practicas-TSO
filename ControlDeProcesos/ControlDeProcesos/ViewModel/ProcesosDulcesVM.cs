using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlDeProcesos.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AdrfrankLibrary.Core;
using System.Timers;
using System.Diagnostics;

namespace ControlDeProcesos.ViewModel
{
    public class ProcesosDulcesVM: NotifyPropetryAdapter
    {

        ICommand iniciar, generar, detener,salir;
        Timer timer;
        Planificador plan;

        int numeroProcesos =20;
        public int NumeroProcesos
        {
            get
            {
                return numeroProcesos;
            }
            set
            {
                numeroProcesos = value;
                generarProcesos();
            }
        }

        public ICommand Iniciar
        {
            get { return iniciar == null ? iniciar = new ActionCommand(iniciarProcesos) : iniciar; }
        }

        public ICommand Generar
        {
            get
            {
                return generar == null?generar= new ActionCommand(generarProcesos): generar;
            }
        }

        public ICommand DetenerProceso
        {
            get
            {
                return detener == null ? detener = new ActionCommand(detenerProcesos) : detener;
            }
        }

        public ICommand Salir
        {
            get { return salir == null ? salir = new ActionCommand(salirProcesos) : salir; }
        }

       public double PorcentajeCompletado {
            get {
                return plan.Ejecucion.FirstOrDefault() == null ? 0 : plan.Ejecucion.First().PorcentajeCompletado; }
        }

        public ObservableCollection<Proceso> Nuevo { get { return new ObservableCollection<Proceso>(plan.Nuevo); } }
        public ObservableCollection<Proceso> Listo { get { return new ObservableCollection<Proceso>(plan.Listo); } }
        public ObservableCollection<Proceso> Ejecucion { get { return new ObservableCollection<Proceso>(plan.Ejecucion); } }
        public ObservableCollection<Proceso> Terminado { get { return new ObservableCollection<Proceso>(plan.Terminado); } }
        public ObservableCollection<Proceso> Bloqueado { get { return new ObservableCollection<Proceso>(plan.Bloqueado); } }




        void generarProcesos() {
            plan.GenerarProcesos(numeroProcesos);
            UpdateUI();

        }

        void iniciarProcesos() {
            plan.Iniciar();
            timer.Start();

        }

        void detenerProcesos() {
            plan.DetenerProceso();
            UpdateUI();

        }

        private void salirProcesos()
        {
            timer.Stop();
            plan.Detener();
            UpdateUI();

        }



        public ProcesosDulcesVM()
        {

            timer = new Timer(10);
            timer.Elapsed += Timer_Elapsed;
            plan = new Planificador();
            plan.Fabrica = new FabricaDulce();
            plan.GenerarProcesos(20);
            plan.ProcesosTerminados += Plan_ProcesosTerminados;
        

            
        }

        void UpdateUI() {
            OnPropertyChanged("Nuevo");
            OnPropertyChanged("Listo");
            OnPropertyChanged("Ejecucion");
            OnPropertyChanged("Terminado");
            OnPropertyChanged("Bloqueado");
            OnPropertyChanged("PorcentajeCompletado");
        }

        private void Plan_ProcesosTerminados(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            plan.NextTime(e.SignalTime);
            UpdateUI();            
            Trace.WriteLine(e.SignalTime);
        }
    }
}
