﻿namespace RecipePortal.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int RecipeId { get; set; }
    }
}