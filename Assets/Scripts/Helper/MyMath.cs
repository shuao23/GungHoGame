public static class MyMath
{
    public static float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
}