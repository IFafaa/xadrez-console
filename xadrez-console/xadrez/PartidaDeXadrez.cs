using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using xadrez;
using xadrez_console.tabuleiro;

namespace xadrez_console.xadrez {
    internal class PartidaDeXadrez {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas{ get; set; }
        private HashSet<Peca> capturadas { get; set; }
        public bool xeque { get; private set; }
        public Peca vulneravelEnPassant { get; private set; }


        public PartidaDeXadrez(Tabuleiro tab) {
            this.tab = tab;
            turno = 0;
            jogadorAtual = Cor.BRANCO;
            terminada = false;
            pecas = new HashSet<Peca> { };
            capturadas = new HashSet<Peca> { };
            xeque = false;
            vulneravelEnPassant = null;
            colocarPecas();
        }

        public Peca executaMovimento(Posicao origem, Posicao destino) {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQntMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if(pecaCapturada != null) {
                capturadas.Add(pecaCapturada);
            }

            // #jogadaespecial roque pequeno
            if(p is Rei && destino.coluna == origem.coluna + 2) {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna + 1);
                Peca torre = tab.retirarPeca(origemTorre);
                torre.incrementarQntMovimentos();
                tab.colocarPeca(torre, destinoTorre);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2) {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna - 1);
                Peca torre = tab.retirarPeca(origemTorre);
                torre.incrementarQntMovimentos();
                tab.colocarPeca(torre, destinoTorre);
            }

            // #jogadaespecial enpassant
            if(p is Peao) {
                if(origem.coluna != destino.coluna && pecaCapturada == null) {
                    Posicao posP;
                    if(p.cor == Cor.BRANCO) {
                        posP = new Posicao(destino.linha + 1, destino.coluna);
                    }
                    else {
                        posP = new Posicao(destino.linha - 1, destino.coluna);
                    }
                    pecaCapturada = tab.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void realizaJogada(Posicao origem, Posicao destino) {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorAtual)) {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = tab.peca(destino);

            // #jogadaespecial promocao
            if(p is Peao) {
                if(p.cor == Cor.BRANCO && destino.linha == 0 || p.cor == Cor.PRETO && destino.linha == 7) {
                    p = tab.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Dama(p.cor, tab);
                    tab.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }

            if (estaEmXeque(adversaria(jogadorAtual))) {
                xeque = true;
            }
            else {
                xeque = false;
            }

            if (testeXequemate(adversaria(jogadorAtual))) {
                terminada = true;
            }
            else {
                turno++;
                mudaJogador();
            }

            // #jogadaespecial enpassant
            if (p is Peao && ((destino.linha == origem.linha - 2) || (destino.linha == origem.linha + 2))) {
                vulneravelEnPassant = p;
            }
            else {
                vulneravelEnPassant = null;
            }
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada) {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQntMovimentos();
            if(pecaCapturada != null) {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2) {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna + 1);
                Peca torre = tab.retirarPeca(destinoTorre);
                torre.decrementarQntMovimentos();
                tab.colocarPeca(torre, origemTorre);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2) {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna - 1);
                Peca torre = tab.retirarPeca(destinoTorre);
                torre.decrementarQntMovimentos();
                tab.colocarPeca(torre, origemTorre);
            }

            // #jogadaespecial enpassant
            if (p is Peao) {
                if (origem.coluna != destino.coluna && pecaCapturada == vulneravelEnPassant) {
                    Peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    if(p.cor == Cor.BRANCO) {
                        posP = new Posicao(3, destino.coluna);
                    }
                    else {
                        posP = new Posicao(4, destino.coluna);
                    }
                    tab.colocarPeca(peao, posP);
                }
            }

        }

        public void mudaJogador() {
            if(jogadorAtual == Cor.PRETO) {
                jogadorAtual = Cor.BRANCO;
            }
            else {
                jogadorAtual = Cor.PRETO;
            }
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca) {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }
        private void colocarPecas() {
            colocarNovaPeca('a', 1, new Torre(Cor.BRANCO, tab));
            colocarNovaPeca('b', 1, new Cavalo(Cor.BRANCO, tab));
            colocarNovaPeca('c', 1, new Bispo(Cor.BRANCO, tab));
            colocarNovaPeca('d', 1, new Dama(Cor.BRANCO, tab));
            colocarNovaPeca('e', 1, new Rei(Cor.BRANCO, tab, this));
            colocarNovaPeca('f', 1, new Bispo(Cor.BRANCO, tab));
            colocarNovaPeca('g', 1, new Cavalo(Cor.BRANCO, tab));
            colocarNovaPeca('h', 1, new Torre(Cor.BRANCO, tab));
            colocarNovaPeca('a', 2, new Peao(Cor.BRANCO, tab, this));
            colocarNovaPeca('b', 2, new Peao(Cor.BRANCO, tab, this));
            colocarNovaPeca('c', 2, new Peao(Cor.BRANCO, tab, this));
            colocarNovaPeca('d', 2, new Peao(Cor.BRANCO, tab, this));
            colocarNovaPeca('e', 2, new Peao(Cor.BRANCO, tab, this));
            colocarNovaPeca('f', 2, new Peao(Cor.BRANCO, tab, this));
            colocarNovaPeca('g', 2, new Peao(Cor.BRANCO, tab, this));
            colocarNovaPeca('h', 2, new Peao(Cor.BRANCO, tab, this));

            colocarNovaPeca('a', 8, new Torre(Cor.PRETO, tab));
            colocarNovaPeca('b', 8, new Cavalo(Cor.PRETO, tab));
            colocarNovaPeca('c', 8, new Bispo(Cor.PRETO, tab));
            colocarNovaPeca('d', 8, new Dama(Cor.PRETO, tab));
            colocarNovaPeca('e', 8, new Rei(Cor.PRETO, tab, this));
            colocarNovaPeca('f', 8, new Bispo(Cor.PRETO, tab));
            colocarNovaPeca('g', 8, new Cavalo(Cor.PRETO, tab));
            colocarNovaPeca('h', 8, new Torre(Cor.PRETO, tab));
            colocarNovaPeca('a', 7, new Peao(Cor.PRETO, tab, this));
            colocarNovaPeca('b', 7, new Peao(Cor.PRETO, tab, this));
            colocarNovaPeca('c', 7, new Peao(Cor.PRETO, tab, this));
            colocarNovaPeca('d', 7, new Peao(Cor.PRETO, tab, this));
            colocarNovaPeca('e', 7, new Peao(Cor.PRETO, tab, this));
            colocarNovaPeca('f', 7, new Peao(Cor.PRETO, tab, this));
            colocarNovaPeca('g', 7, new Peao(Cor.PRETO, tab, this));
            colocarNovaPeca('h', 7, new Peao(Cor.PRETO, tab, this));
        }

        public HashSet<Peca> pecasCapturadas(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach(Peca peca in capturadas) {
                if(peca.cor == cor) {
                    aux.Add(peca);
                }
            }
            return aux;
        }


        public HashSet<Peca> pecasEmJogo(Cor cor) {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca peca in pecas) {
                if (peca.cor == cor) {
                    aux.Add(peca);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        public void validarPosicaoDeOrigem(Posicao pos) {
            if(tab.peca(pos) == null) {
                throw new TabuleiroException("Não existe peça na posicao de origem escolhida!");
            }
            if(jogadorAtual != tab.peca(pos).cor) {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis()) {
                throw new TabuleiroException("Não há movimentos possiveis para a peça de origem escolhida!");
            }
        }

        public void validarPosicaoDeDestino(Posicao origem, Posicao destino) {
            if (!tab.peca(origem).movimentoPossivel(destino)) {
                throw new TabuleiroException("Não é possível mover a peça pra esse destino!");
            }
        }

        private Cor adversaria(Cor cor) {
            if(cor == Cor.PRETO) {
                return Cor.BRANCO;
            }
            else {
                return Cor.PRETO;
            }
        }

        private Peca rei(Cor cor) {
            foreach(Peca x in pecasEmJogo(cor)) {
                if(x is Rei) {
                    return x;
                }
            }
            return null;
       }

        public bool estaEmXeque(Cor cor) {
            Peca R = rei(cor);
            if (R == null) {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }
            foreach (Peca x in pecasEmJogo(adversaria(cor))) {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna]) {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequemate(Cor cor) {
            if (!estaEmXeque(cor)) {
                return false;
            }
            foreach (Peca x in pecasEmJogo(cor)) {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < tab.linhas; i++) {
                    for (int j = 0; j < tab.colunas; j++) {
                        if (mat[i, j]) {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
