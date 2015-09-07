#include <iostream>
#include <cstdlib>
#include <ctime>
#include <conio.h>
#include <queue>

#define SALIR 1
#define OK  0

using namespace std;

string tipos[] = {"Chocolate","Menta","Vainilla","Fresa","Mango","Mora","Cacahuate","Avellana","Chile","Tamarindo"};

class Proceso{
private:
    int n,lote,tiempo;
    bool mostrarLote=false;
    int tipo;
public:
    Proceso(int n,int lote, int tiempo,int tipo){
        this->n = n;
        this->lote = lote;
        this->tiempo = tiempo;
        this->tipo = tipo;
    }
    void mostrar(){
        cout<<endl;
        if(mostrarLote)
        cout<<"Lote #"<<lote<<endl;
        cout<<"Dulce Id: "<<n<<endl;
        cout<<"Tipo: "<<tipos[tipo]<<endl;
        cout<<"Tiempo: "<<tiempo<<" seg."<<endl;
    }
    int getId(){ return n;}
    int getLote(){return lote;}
    int getTiempo(){return tiempo;}
    string getTipo(){return tipos[tipo];}

    int Ejecutar(){
        int exit_d=OK,next=OK;
        mostrar();
        clock_t time_end;
        time_end = clock() + tiempo * CLOCKS_PER_SEC;
        long cont=0;
        while (clock() < time_end && next==OK)
        {
            if(cont++% 10000==0)
                cout<<".";
            if(kbhit()){
                char c=getch();
                switch(c){
                case 'e':
                    cout<<"Error de dulce"<<endl;
                    next=SALIR;
                    break;
                case 's':
                    next=SALIR;
                    exit_d=SALIR;
                }
            }
        }
        return exit_d;
    }
};

class Manejador{
private:
    int nproc,proclote=4,exit_d=0;
    queue<Proceso> nuevo,listo,ejecucion,terminado,bloqueado;
public:
    Manejador(int nproc, int proclote){
        this->nproc = nproc;
        this->proclote = proclote;
    }
    void mostrarBandejas(){
        cout<<endl;
        cout<<"Listos: "<<listo.size()<<" Ejecucion: "<<ejecucion.size()<<" Terminados: "<<terminado.size()<<endl;
    }
    void mostrarEncabezado(){
        cout<<"Numero de procesos: "<<nproc<<endl;
        cout<<"--------------------"<<endl;
    }
    void generarListos(){
        exit_d=0;
        int sleeptime=0,lot=1;
        for(int i=1; i <= nproc && !exit_d; ++i){
            if(i%proclote==0)
                lot++;
            sleeptime = rand() % 5 + 1;
            listo.push(Proceso(i,lot,sleeptime,rand() % 10));
        }
    }
    void ejecutarListos(){
        while( listo.size() > 0 ){
            system("cls");
            mostrarEncabezado();
            ejecucion.push(listo.front());
            listo.pop();
            mostrarBandejas();
            exit_d = ejecucion.front().Ejecutar();
            terminado.push(ejecucion.front());

            if(exit_d == SALIR)
            {
                cout<<endl<<"Procesamiento detenido"<<endl;
                break;
            }

            ejecucion.pop();
        }
    }
    void iniciar(){
        mostrarEncabezado();
        generarListos();
        ejecutarListos();
    }
};

ostream& operator<<(ostream& os, Proceso& p){
    cout<<"Id: "<<p.getId()<<" "<<p.getLote()<<" "<<p.getTipo()<<endl;
}

int main()
{
    srand (time(NULL));
    Manejador man(rand() % 30 + 20, 4);
    man.iniciar();

    return 0;
}
