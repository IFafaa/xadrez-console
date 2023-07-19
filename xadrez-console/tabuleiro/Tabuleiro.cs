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

        public Peca peca(Posicao pos) {
            return _pecas[pos.linha, pos.coluna];
        }

        public Peca retirarPeca(Posicao pos) {
            Peca peca = this.peca(pos);
            if(peca == null) {
                return null;
            }
            peca.posicao = null;
            _pecas[pos.linha, pos.coluna] = null;
            return peca;
        }

        public void colocarPeca(Peca peca, Posicao pos) {
            if (existePeca(pos)) {
                throw new TabuleiroException("Já existe uma peça nessa posição!");
            }
            _pecas[pos.linha, pos.coluna] = peca;
            peca.posicao = pos;
        }

        public bool existePeca(Posicao pos) {
            validarPosicao(pos);
            return peca(pos) != null;
        }
        public bool posicaoValida(Posicao pos) {
            return !(pos.linha < 0 || pos.linha > linhas - 1 || pos.coluna < 0 || pos.coluna > colunas - 1);
        }

        public void validarPosicao(Posicao pos) {
            if (!posicaoValida(pos)) {
                throw new TabuleiroException("Posição inválida!");
            }
        }
    }
}
