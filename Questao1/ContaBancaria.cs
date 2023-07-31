using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        private int Numero;
        private string Titular;
        private double? Saldo;

        public ContaBancaria(int numero, string titular, double? saldo = 0)
        {
            Numero = numero;
            Titular = titular;
            Saldo = saldo;
        }

        public void Deposito(double quantia)
        {
            Saldo += quantia;  
        }

        public void Saque(double quantia)
        {
           const double TAXA_SAQUE = 3.50;

           Saldo -= quantia;
           Saldo -= TAXA_SAQUE;
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo:f2}";
        }
    }   
}
