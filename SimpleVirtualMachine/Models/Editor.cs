using System;
namespace SimpleVirtualMachine.Models
{
    public class Editor
    {
        public Editor()
        {
        }

        public void Edit()
        {
            Print.Show.AvabileOpcodes();

            //unsigned int cntInstrukcja = 0;
            //short iRozkaz = 0;
            //short iOperacja = 0;
            //short itIndeks[2];
            //int iSlowo = 0;
            //bool bBlad = true;

            //while (iOperacja != opZapisz)
            //{
            //    cout << "Podaj operacje: " << endl << "> ";
            //    cin >> iOperacja;
            //    switch (iOperacja)
            //    {
            //        case opDodaj:
            //        case opOdejmij:
            //        case opMnoz:
            //        case opDziel:
            //        case opPorownaj:
            //            {
            //                bBlad = true;
            //                while (bBlad)
            //                {
            //                    cout << "Podaj indeks pierwszego rejestru (0 - 63): " << endl << "> ";
            //                    cin >> itIndeks[r1];
            //                    if ((itIndeks[r1] < 0) || (itIndeks[r1] > 63))
            //                    {
            //                        cout << "Bledny indeks pierwszego rejestru!" << endl;
            //                    }
            //                    else
            //                        bBlad = false;
            //                }

            //                bBlad = true;
            //                while (bBlad)
            //                {
            //                    cout << "Podaj indeks drugiego rejestru (0 - 63): " << endl << "> ";
            //                    cin >> itIndeks[r2];
            //                    if ((itIndeks[r2] < 0) || (itIndeks[r2] > 63))
            //                    {
            //                        cout << "Bledny indeks drugiego rejestru!" << endl;
            //                    }
            //                    else
            //                        bBlad = false;
            //                }

            //                iRozkaz = (itIndeks[r2] << 10) | (itIndeks[r1] << 4) | iOperacja;

            //                plik.write((char*)&iRozkaz, 2);
            //                cout << "Operacja " << szR[iOperacja] << ": " << itIndeks[r2] << "*2^10 + " << itIndeks[r1] << "*2^4 + " << iOperacja << " = " << iRozkaz << endl;
            //                cntInstrukcja++;
            //                break;
            //            }
            //        case opWyswietl:
            //        case opPobierz:
            //            {
            //                bBlad = true;
            //                while (bBlad)
            //                {
            //                    cout << "Podaj indeks rejestru (0 - 63): " << endl << "> ";
            //                    cin >> itIndeks[r1];
            //                    if ((itIndeks[r1] < 0) || (itIndeks[r1] > 63))
            //                    {
            //                        cout << "Bledny indeks rejestru!" << endl;
            //                    }
            //                    else
            //                        bBlad = false;
            //                }

            //                itIndeks[r2] = 0;

            //                iRozkaz = (itIndeks[r2] << 10) | (itIndeks[r1] << 4) | iOperacja;

            //                plik.write((char*)&iRozkaz, 2);
            //                cout << "Operacja " << szR[iOperacja] << ": " << itIndeks[r1] << "*2^4 + " << iOperacja << " = " << iRozkaz << endl;
            //                cntInstrukcja++;
            //                break;
            //            }
            //        case opSkocz:
            //            {
            //                bBlad = true;
            //                while (bBlad)
            //                {
            //                    cout << "Podaj warunek skoku (0 - 6): " << endl << "> ";
            //                    cin >> itIndeks[r1];
            //                    if ((itIndeks[r1] < 0) || (itIndeks[r1] > 6))
            //                    {
            //                        cout << "Bledny warunek skoku!" << endl;
            //                    }
            //                    else
            //                        bBlad = false;
            //                }

            //                itIndeks[r2] = 0;

            //                bBlad = true;
            //                while (bBlad)
            //                {
            //                    cout << "Podaj wartosc skoku: " << endl << "> ";
            //                    cin >> iSlowo;
            //                    if (cin.fail())
            //                    {
            //                        cout << "Bledna wartosc skoku!" << endl;
            //                    }
            //                    else
            //                        bBlad = false;
            //                }

            //                iRozkaz = (itIndeks[r2] << 10) | (itIndeks[r1] << 4) | iOperacja;

            //                plik.write((char*)&iRozkaz, 2);
            //                plik.write((char*)&iSlowo, 4);
            //                cout << "Operacja " << szR[iOperacja] << ": " << itIndeks[r2] << "*2^10 + " << itIndeks[r1] << "*2^4 + " << iOperacja << " = " << iRozkaz << " SKOK: " << iSlowo << endl;
            //                cntInstrukcja++;
            //                break;
            //            }
            //        case opWczytaj:
            //            {
            //                bBlad = true;
            //                while (bBlad)
            //                {
            //                    cout << "Podaj indeks rejestru (0 - 63): " << endl << "> ";
            //                    cin >> itIndeks[r1];
            //                    if ((itIndeks[r1] < 0) || (itIndeks[r1] > 63))
            //                    {
            //                        cout << "Bledny indeks rejestru!" << endl;
            //                    }
            //                    else
            //                        bBlad = false;
            //                }
            //                itIndeks[r2] = 0;

            //                bBlad = true;
            //                while (bBlad)
            //                {
            //                    cout << "Podaj wartosc stalej: " << endl << "> ";
            //                    if (!cin >> iSlowo)
            //                    {
            //                        cout << "Bledna wartosc skoku!" << endl;
            //                    }
            //                    else
            //                        bBlad = false;
            //                }

            //                iRozkaz = (itIndeks[r2] << 10) | (itIndeks[r1] << 4) | iOperacja;
            //                plik.write((char*)&iRozkaz, 2);
            //                plik.write((char*)&iSlowo, 4);
            //                cout << "Operacja " << szR[iOperacja] << ": " << itIndeks[r2] << "*2^10 + " << itIndeks[r1] << "*2^4 + " << iOperacja << " = " << iRozkaz << " STALA: " << iSlowo << endl;
            //                cntInstrukcja++;
            //                break;
            //            }
            //        case opZakoncz:
            //            {
            //                itIndeks[r1] = 0;
            //                itIndeks[r2] = 0;

            //                iRozkaz = (itIndeks[r2] << 10) | (itIndeks[r1] << 4) | iOperacja;
            //                plik.write((char*)&iRozkaz, 2);
            //                cntInstrukcja++;
            //                break;
            //            }
            //        default:
            //            {
            //                cout << "Nieprawidlowa operacja!" << endl;
            //                break;
            //            }
            //    }
            //}
            //plikZamknij(plik);
        }
    }
}
