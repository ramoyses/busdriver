using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

public enum Rank
{
    
    Two=2, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
    Jack, Queen, King, Ace
}

public class Card
{
    public Suit Suit { get; }
    public Rank Rank { get; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}

public class Deck
{
    private List<Card> cards;
    private Random rng;

    public Deck()
    {
        cards = new List<Card>();
        rng = new Random();

        // Populate the deck with 52 cards
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(suit, rank));
            }
        }
    }

    public void Shuffle()
    {
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    public void PrintDeck()
    {
        foreach (Card card in cards)
        {
            Console.WriteLine(card);
        }
    }

    public List<Card> Deal(int numCards)
    {
        List<Card> dealtCards = new List<Card>();

        for (int i = 0; i < numCards; i++)
        {
            if (cards.Count == 0)
            {
                throw new Exception("No more cards in the deck.");
            }

            Card card = cards[0];
            cards.RemoveAt(0);
            dealtCards.Add(card);
        }

        return dealtCards;
    }
}


class Program
{
    static void Main(string[] args)
    {
        List<int> distribution = new List<int>();
        for (int i = 0; i < 1e5; i++)
        {
            distribution.Add(RunGame());
            //float p = distribution.Where(x => x.Equals(40)).Count()/distribution.Count();
            int e = distribution.Where(x => x.Equals(40)).Count();
            int n = distribution.Count();
            Console.WriteLine("e {0} - n {1} - p40 {2:N4} - pm {3:N4}", e, n, (double) 100*e/ n, 
                (double)distribution.Average());
    }
    }

    static int RunGame()
    {
        Deck deck = new Deck();
        int points=0;
        deck.Shuffle();

        List<Card> hand = deck.Deal(6);
        //Console.WriteLine("Initial Hand:");
        //PrintCards(hand);
        int new_cards = DealMoreCards(hand);
        points += new_cards;
        while (new_cards > 0){
            //Console.WriteLine("\nFigure! Dealing an extra {0} cards.", new_cards);
            List<Card> extraCards = deck.Deal(new_cards);
            //Console.WriteLine("\nExtra Cards:");
            //PrintCards(extraCards);
            hand.AddRange(extraCards);
            new_cards = DealMoreCards(extraCards);
            points += new_cards;
        }

        return points;
        
    }

    static void PrintCards(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            Console.WriteLine(card);
        }
    }

    static int DealMoreCards(List<Card> cards)
    {
        int new_cards = 0;
        
        foreach (Card card in cards)
        {
            if (card.Rank>Rank.Ten)
            {
                new_cards+=card.Rank-Rank.Ten;
            }
        }
        return new_cards;
    }
}