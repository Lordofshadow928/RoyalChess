public static class CoinConverter
{
    // Number of foods required to earn one coin.
    public const int FoodPerCoin = 10;
    // Converts collected food into earned coins.
    // Example:
    // 400 food -> 40 coins
    // 313 food -> 31 coins
    // 9 food -> 0 coins
    public static int ConvertFoodToCoins(int foodAmount)
    {
        if (foodAmount <= 0)
            return 0;

        return foodAmount / FoodPerCoin;
    }
}