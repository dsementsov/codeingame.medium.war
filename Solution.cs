using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;

class Solution
{
    public static Queue<string> hand1;
    public static Queue<string> hand2;
    public static Queue<string> bank1 = new Queue<string>();
    public static Queue<string> bank2 = new Queue<string>();
    static bool gameOver = false;
    static string gameOutcome = "";

    static void Main(string[] args)
    {
        int roundsNumber = 0;
        string[] card1, card2;
        int n = int.Parse(Console.ReadLine()); // the number of cards for player 1
        card1 = new string[n];
        for (int i = 0; i < n; i++)
        {
            string cardp1 = Console.ReadLine(); // the n cards of player 1
            card1[i] = cardp1;
        }

        int m = int.Parse(Console.ReadLine()); // the number of cards for player 2
        card2 = new string[m];
        for (int i = 0; i < m; i++)
        {
            string cardp2 = Console.ReadLine(); // the m cards of player 2
            card2[i] = cardp2;
        }
        hand1 = new Queue<string>(card1);       // assigning hands
        hand2 = new Queue<string>(card2);

        do
        {
            roundsNumber++;
            CheckForGameOutcome();
            Console.Error.WriteLine("turn no " + roundsNumber);
            Turn();
            CheckForGameOutcome();
        } while (gameOver == false);
        string solution = gameOutcome + " " + roundsNumber.ToString();
        if (gameOutcome == "PAT")
            solution = gameOutcome;
       Console.WriteLine(solution);
    }
    // simulates one turn of the game
    public static void Turn()
    {
        // comparing cards
        int winningHand = CheckWinningHand(hand1.Peek(), hand2.Peek());
        Console.Error.WriteLine("before turn h1top " + hand1.Peek() + " h2top " + hand2.Peek());
        Console.Error.WriteLine("winninghand " + winningHand);
        switch (winningHand)
        {
            case 0:
                War();
                break;
            case 1:
                bank1.Enqueue(hand1.Dequeue());
                bank2.Enqueue(hand2.Dequeue());
                BankUp(hand1);
                break;
            case 2:
                bank1.Enqueue(hand1.Dequeue());
                bank2.Enqueue(hand2.Dequeue());
                BankUp(hand2);
                break;
        }
    }

    //check for game outcome 
    public static void CheckForGameOutcome()
    {
        if (gameOver == true)
            return;

        if (!hand1.Any())
        {
            gameOver = true;
            gameOutcome = "2";
        }

        if (!hand2.Any())
        {
            gameOver = true;
            gameOutcome = "1";
        }
    }

    public static void BankUp(Queue<string> bankingHand)
    {
        while (bank1.Any())
        {
            Console.Error.WriteLine("banking up bank1 " + bank1.Peek());
            bankingHand.Enqueue(bank1.Dequeue());
        }

        while (bank2.Any())
        {
            Console.Error.WriteLine("bank2 count is " + bank2.Count());
            Console.Error.WriteLine("banking up bank2 " + bank2.Peek());
            bankingHand.Enqueue(bank2.Dequeue());
        }

    }


    //take two cards, return which oce is higher
    public static int CheckWinningHand(string c1, string c2)
    {
        if (DeckOfCards.cardValue[c1[0]] > DeckOfCards.cardValue[c2[0]])
            return 1;
        if (DeckOfCards.cardValue[c1[0]] < DeckOfCards.cardValue[c2[0]])
            return 2;
        if (DeckOfCards.cardValue[c1[0]] == DeckOfCards.cardValue[c2[0]])
            return 0;
        return 0;
    }



    public static void War()
    {
        if ((hand1.Count() < 4) || (hand2.Count() < 4))
        {
            gameOver = true;
            gameOutcome = "PAT";
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            Console.Error.WriteLine("bank1 + {0} bank2 + {1}", hand1.Peek(), hand2.Peek());
            bank1.Enqueue(hand1.Dequeue());
            bank2.Enqueue(hand2.Dequeue());
        }
        Turn();
    }
}

public static class DeckOfCards
{
    public static Dictionary<char, int> cardValue = new Dictionary<char, int>();
    static DeckOfCards()
    {
        cardValue.Add('2', 0);
        cardValue.Add('3', 1);
        cardValue.Add('4', 2);
        cardValue.Add('5', 3);
        cardValue.Add('6', 4);
        cardValue.Add('7', 5);
        cardValue.Add('8', 6);
        cardValue.Add('9', 7);
        cardValue.Add('1', 8);
        cardValue.Add('J', 9);
        cardValue.Add('Q', 10);
        cardValue.Add('K', 11);
        cardValue.Add('A', 12);
    }
}

