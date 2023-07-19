using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrez_console.tabuleiro {
    internal class Tabuleiro {
        public int linhas { get; set; }
        public int colunas { get; set; }
        private Peca[,] _pecas;
    
        public Tabuleiro(int linhas, int colunas) {
            this.linhas = linhas;
            this.colunas = colunas;
            _pecas = new Peca[linhas, colunas];
        }
    }
}
