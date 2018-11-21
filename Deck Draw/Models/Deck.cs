using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Deck_Draw.Models
{
    public class Deck
    {
        public string DeckID { get; set; }
    }

    public class Card : Deck
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }

    public class DeckTable : DbContext
    {
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Card> Cards { get; set; }
    }
}