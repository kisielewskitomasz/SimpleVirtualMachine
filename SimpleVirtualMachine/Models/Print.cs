using System;

namespace SimpleVirtualMachine.Models
{
    public static class Print
    {
        public static class Show
        {
            public static void Description()
            {
                Console.WriteLine("+---------------------------OPIS MASZYNY WIRTUALNEJ---------------------------+");
                Console.WriteLine("| Maszyna zawiera:                                                            |");
                Console.WriteLine("| * 64 4-bajtowe rejestry pamieci indeksowane od 0                            |");
                Console.WriteLine("| * 4-bajtowy rejestr instrukcji                                              |");
                Console.WriteLine("| * 1-bajtowy rejestr flagi                                                   |");
                Console.WriteLine("| - W kazdym rejestrze pamieci moze byc przechowywana liczba (int) ze znakiem |");
                Console.WriteLine("| - Rejestr instrukcji przechowuje liczbe (int) bez znaku informujaca o tym,  |");
                Console.WriteLine("|   ktora z kolei jest wykonwyana instrukcja. Jest autoinkrementowany,        |");
                Console.WriteLine("|   operacja skoku zmienia odpowiednio jego wartosc                           |");
                Console.WriteLine("| - Trojstanowy rejestr flagi przechowuje informacje o wyniku ostatniej       |");
                Console.WriteLine("|   operacji arytmetycznej (dodawanie, odejmowanie, mnozenie, dzielenie,      |");
                Console.WriteLine("|   porownanie) i w zaleznosi od jej rezultatu przyjmuje wartosc:             |");
                Console.WriteLine("|    * flaga D (1) dla dodatniego wyniku                                      |");
                Console.WriteLine("|    * flaga Z (0) dla zera                                                   |");
                Console.WriteLine("|    * flaga U (-1) dla ujemnego wyniku                                       |");
                Console.WriteLine("|   Ta informacja moze byc uzyta przy wykonywaniu operacji skoku              |");
                Console.WriteLine("| - Na poczatku pracy Maszyny Wirtualnej wszystkie rejestry sa wyzerowane.    |");
                Console.WriteLine("|                                                                             |");
                Console.WriteLine("| Operacje:                                                                   |");
                Console.WriteLine("| - Kazda operacja to 2-bajtowa liczba bez znakuw formacie little endian      |");
                Console.WriteLine("|   i ewentualna liczba 4-bajtowa (int) ze znakiem.                           |");
                Console.WriteLine("| - Operacje mozna podzielic na 3 sekcje: R2_R1_OP                            |");
                Console.WriteLine("|    * R2 - 6 bitow - indeks drugiego rejestru pamieci                        |");
                Console.WriteLine("|    * R1 - 6 bitow - indeks pierwszego rejestru pamieci                      |");
                Console.WriteLine("|    * OP - 4 bity - kod operacji                                             |");
                Console.WriteLine("| - Dla operacji o kodach 6 i 7 (skocz i wczytaj) za 2-bajtowa operacja stoi: |");
                Console.WriteLine("|    * INT - 4 bajty - stala skoku albo stala wczytywana do rejestru          |");
                Console.WriteLine("| - Dla operacji o kodach: 6, 7, 8, 9, 10 wartosc R2 w liczbie operacji to 0. |");
                Console.WriteLine("| - Dla operacji o kodzie: 10 wartosc R1 w liczbie operacji to 0.             |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
            }

            public static void AvabileOpcodes()
            {
                Console.WriteLine("+---------+--------+-----------DOSTEPNE OPERACJE------------------------------+");
                Console.WriteLine("| Format: | R2(6b) | R1(6b) | OP(4b) | INT(4B)   (tylko dla skocz i wczytaj)  |");
                Console.WriteLine("+---------+--------+--------+----+---+--------------------+-------------------+");
                Console.WriteLine("| R2, R1: Indeks rejestru (0-63) | OP: Kod rozkazu (0-10) | INT: liczba 4B    |");
                Console.WriteLine("+----+----------+----------------+----+-------------------+-------------------+");
                Console.WriteLine("| Op | Rozkaz   | Opis                | Format rozkazu SKOCZ: 0_R1_6_INT      |");
                Console.WriteLine("+----+----------+---------------------+-----+---------------------------------+");
                Console.WriteLine("|  0 | dodaj    | R1:=R1+R2           |  R1 | WARUNEK skoku (stan flagi)      |");
                Console.WriteLine("|  1 | odejmij  | R1:=R1-R2           +-----+---------------------------------+");
                Console.WriteLine("|  2 | mnoz     | R1:=R1*R2           |   0 | Zawsze                          |");
                Console.WriteLine("|  3 | dziel    | R1:=R1/R2 R2:=R1%R2 |   1 | Ustanwiona Z (zero)             |");
                Console.WriteLine("|  4 | porownaj | R3:=R1-R2           |   2 | Nieustawniona  Z (nie zero)     |");
                Console.WriteLine("|  5 | kopiuj   | R1:=R2              |   3 | Ustawiona D (dodatnia)          |");
                Console.WriteLine("|  6 | skocz    | skocz do instrukcji |   4 | Ustawiona U (ujemna)            |");
                Console.WriteLine("|  7 | wczytaj  | R1:= stala INT      |   5 | Nieustawniona U (nieujemna)     |");
                Console.WriteLine("|  8 | pobierz  | R1:= z klawiatury   |   6 | Nieustawniona D (niedodatania)  |");
                Console.WriteLine("|  9 | wyswietl | wypisz R1           +-----+---------------------------------+");
                Console.WriteLine("| 10 | zakoncz  | zakoncz program     | INT | skok o INT rozkazow             |");
                Console.WriteLine("+----+----------+---------------------+-----+---------------------------------+");
                Console.WriteLine("|           Operacje edytora          | Format rozkazu WCZYTAJ: 0_R1_7_INT    |");
                Console.WriteLine("+----+----------+---------------------+-----+---------------------------------+");
                Console.WriteLine("| 11 | zapisz   | zapisz plik         | INT | wczytaj do R1 stala INT         |");
                Console.WriteLine("+----+----------+---------------------+-----+---------------------------------+");
            }

            public static void ProjectExample()
            {
                Console.WriteLine("+-----------------------PRZYKLAD Z ZADANIA PROJEKTOWEGO-----------------------+");
                Console.WriteLine("| Nazwa pliku: projekt.bin                                                    |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
                Console.WriteLine("| * Pobierz liczbe z klawiatury do rejestru 0 (0*2^4+8 = 8)                   |");
                Console.WriteLine("| * Porownaj rejestr 0 z rejestrem 2 (2*2^10+0*2^4+4 = 2052)                  |");
                Console.WriteLine("| * Skocz o 2 instrukcje do przodu jesli nie zero (2*2^4+6 = 38 SKOK: 2)      |");
                Console.WriteLine("| * Skoncz program (10 = 10)                                                  |");
                Console.WriteLine("| * Do rejestru 1 dodaj rejestr 0 (0*2^10+1*2^4+0 = 16)                       |");
                Console.WriteLine("| * Wypisz rejestr 1 (1*2^4+9 = 25)                                           |");
                Console.WriteLine("| * Skocz o 6 instrukcji do tylu (0*2^4+6 = 6)                                |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
            }

            public static void Adding2Int()
            {
                Console.WriteLine("+----------------------------DODAWANIE DWOCH LICZB----------------------------+");
                Console.WriteLine("| Nazwa pliku: dod_ab.bin                                                     |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
                Console.WriteLine("| * Pobierz liczbe z klawiatury do rejestru 0 (0*2^4 + 8 = 8)                 |");
                Console.WriteLine("| * Pobierz liczbe z klawiatury do rejestru 1 (1*2^4 + 8 = 24)                |");
                Console.WriteLine("| * Do rejestru 0 dodaj rejestr 1 (1*2^10 + 0*2^4 + 0 = 1024)                 |");
                Console.WriteLine("| * Wypisz rejestr 0 (0*2^4 + 9 = 9)                                          |");
                Console.WriteLine("| * Skoncz program (10 = 10)                                                  |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
            }

            public static void Dividing2Int()
            {
                Console.WriteLine("+----------------------------DZIELENIE DWOCH LICZB----------------------------+");
                Console.WriteLine("| Nazwa pliku: dziel_ab.bin                                                   |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
                Console.WriteLine("| * Pobierz liczbe z klawiatury do rejestru 0 (0*2^4 + 8 = 8)                 |");
                Console.WriteLine("| * Pobierz liczbe z klawiatury do rejestru 1 (1*2^4 + 8 = 24)                |");
                Console.WriteLine("| * Podziel rejestr 0 przez rejestr 1 (1*2^10 + 0*2^4 + 3 = 1027)             |");
                Console.WriteLine("| * Wypisz rejestr 0 (0*2^4 + 9 = 9)                                          |");
                Console.WriteLine("| * Wypisz rejestr 1 (1*2^4 + 9 = 25)                                         |");
                Console.WriteLine("| * Skoncz program (10 = 10)                                                  |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
            }

            public static void BigestFrom3Ints()
            {
                Console.WriteLine("+--------------------------------NAJWIEKSZA Z 3-------------------------------+");
                Console.WriteLine("| Nazwa pliku: max_3.bin                                                      |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
                Console.WriteLine("| * Pobierz liczbe z klawiatury do rejestru 0 (0*2^4 + 8 = 8)                 |");
                Console.WriteLine("| * Pobierz liczbe z klawiatury do rejestru 1 (1*2^4 + 8 = 24)                |");
                Console.WriteLine("| * Pobierz liczbe z klawiatury do rejestru 2 (2*2^4 + 8 = 40)                |");
                Console.WriteLine("| * Porownaj rejestr 0 z rejestrem 1 (1*2^10 + 0*2^4 + 4 = 1028)              |");
                Console.WriteLine("| * Skocz o 2 instrukcje do przodu jesli dodatnia (3*2^4 + 6 = 54 SKOK: 2)    |");
                Console.WriteLine("| * Skocz o 4 instrukcje do przodu jesli ujemna (4*2^4 + 6 = 70 SKOK: 4)      |");
                Console.WriteLine("| * Porownaj rejestr 0 z rejestrem 2 (2*2^10 + 0*2^4 + 4 = 2052)              |");
                Console.WriteLine("| * Skocz o 6 instrukcje do przodu jesli dodatnia (3*2^4 + 6 = 54 SKOK: 6)    |");
                Console.WriteLine("| * Skocz o 9 instrukcje do przodu jesli ujemna (4*2^4 + 6 = 70 SKOK: 9)      |");
                Console.WriteLine("| * Porownaj rejestr 1 z rejestrem 2 (2*2^10 + 1*2^4 + 4 = 2068)              |");
                Console.WriteLine("| * Skocz o 5 instrukcje do przodu jesli dodatnia (3*2^4 + 6 = 54 SKOK: 5)    |");
                Console.WriteLine("| * Skocz o 6 instrukcje do przodu jesli ujemna (4*2^4 + 6 = 70 SKOK: 6)      |");
                Console.WriteLine("| * Skoncz program (10 = 10)                                                  |");
                Console.WriteLine("| * Wypisz rejestr 0 (0*2^4 + 9 = 9)                                          |");
                Console.WriteLine("| * Skocz o 2 instrukcje do tylu (0*2^4 + 6 = 6 INT: -2)                      |");
                Console.WriteLine("| * Wypisz rejestr 1 (1*2^4 + 9 = 25)                                         |");
                Console.WriteLine("| * Skocz o 4 instrukcje do tylu (0*2^4 + 6 = 6 INT: -4)                      |");
                Console.WriteLine("| * Wypisz rejestr 2 (1*2^4 + 9 = 41)                                         |");
                Console.WriteLine("| * Skocz o 6 instrukcje do tylu (0*2^4 + 6 = 6 INT: -6)                      |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
            }

            public static void Info()
            {
                Console.WriteLine("+--------------------------------OPIS APLIKACJI-------------------------------+");
                Console.WriteLine("| Aplikacja pozwala stworzyc i zapisac plik programu maszyny wirtualnej (VM)  |");
                Console.WriteLine("| oraz jego wykonanie w VM, ktorej dzialanie jest opisane podczas tworzenia   |");
                Console.WriteLine("| pliku oraz szczegolowy opis dostepny jest w MENU GLOWNYM.                   |");
                Console.WriteLine("| Program jest wykonywany krokowo (instrukcja po instrukcji).                 |");
                Console.WriteLine("| W czasie pracy wyswietlany jest obecny stan Maszyny Wirtualnej              |");
                Console.WriteLine("| Autor: Tomasz Kisielewski EiT K3 s165678                                    |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
            }
        }

        public static class Error
        {
            public static void ErrorDivisionByZero()
            {
                Console.WriteLine("+------------------------------------BLAD-------------------------------------+");
                Console.WriteLine("|                     BLAD WYKONYWANIA: DZIELENIE PRZEZ 0                     |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
            }

            public static void ErrorEof()
            {
                Console.WriteLine("+------------------------------------BLAD-------------------------------------+");
                Console.WriteLine("|                        BLAD WYKONYWANIA: KONIEC PLIKU                       |");
                Console.WriteLine("+-----------------------------------------------------------------------------+");
            }
        }
    }
}
