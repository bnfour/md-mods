using Bnfour.MuseDashMods.RankPreview.Utilities;

namespace Tests;

public class LimitedCapacityDictionaryTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void ThrowsOnZeroAndBelowCapacity(int badCapacity)
    {
        Assert.Throws<ArgumentException>(() => new LimitedCapacityDictionary<int, string>(badCapacity));
    }

    [Fact]
    public void WorksAsIntended()
    {
        var lcd = new LimitedCapacityDictionary<int, int>(8);

        lcd[0] = 0;

        Assert.True(lcd.ContainsKey(0));
        Assert.Equal(0, lcd[0]);
        // add to max capacity
        for (int i = 1; i < 8; i++)
        {
            lcd[i] = i * i;
        }

        for (int i = 0; i < 8; i++)
        {
            Assert.True(lcd.ContainsKey(i));
            Assert.Equal(i * i, lcd[i]);
            // check updating as well
            lcd[i] = i * i * 10;
            Assert.True(lcd.ContainsKey(i));
            Assert.Equal(i * i * 10, lcd[i]);
        }

        // now it starts to remove oldest keys
        lcd[8] = 640;
        Assert.False(lcd.ContainsKey(0));
        Assert.True(lcd.ContainsKey(8));
        Assert.Equal(640, lcd[8]);

        lcd[9] = 810;
        Assert.False(lcd.ContainsKey(1));
        Assert.True(lcd.ContainsKey(9));
        Assert.Equal(810, lcd[9]);

        for (int i = 0; i < 5; i++)
        {
            lcd[10 + i] = (10 + i) * (10 + i) * 10;
        }

        for (int i = 0; i < 7; i++)
        {
            Assert.False(lcd.ContainsKey(i));
        }
        for (int i = 7; i < 15; i++)
        {
            Assert.True(lcd.ContainsKey(i));
            Assert.Equal(i * i * 10, lcd[i]);
            lcd[i] = i;
            Assert.Equal(i, lcd[i]);
        }

        Assert.False(lcd.ContainsKey(15));
    }
}
