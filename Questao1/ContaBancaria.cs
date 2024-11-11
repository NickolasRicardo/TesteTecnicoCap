using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
       public int numero { get; set; }
        
        public string titular { get; set; }
        public double saldo { get; set; }



        public ContaBancaria(int numero, string titular, double depositoInicial = 0)
        {
            this.numero = numero;
            this.titular = titular;
            saldo = depositoInicial;
        }

        public void Deposito(double valorDeposito)
        {
            saldo += valorDeposito;
        }
        
        public void Saque(double valorSaque)
        {
            double valorTaxa = 3.5;
            
            saldo -= valorSaque + valorTaxa ;
        }
    }
}
