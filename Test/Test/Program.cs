Random rnd = new Random();
string[] lines = File.ReadAllLines(@"C:\C#\Test\words.txt");
int num = rnd.Next(5757);
string word = lines[num];

string newWord = "";
int counter = 0;

List<char> wrongPlace = new List<char>();
List<char> bad = new List<char>();


char[] correct = new char[5];

char[] brokenWord = word.ToCharArray();
char[] newBroken = new char[5];
 
Console.WriteLine(word);

while ((!brokenWord.SequenceEqual(newBroken)) || (counter != 5)) 
{
    Console.WriteLine(" ");
    Console.WriteLine("Enter a 5 letter word");
    Console.WriteLine(counter);
    newWord = Console.ReadLine();
    if (newWord.Length != 5)
    {
        Console.WriteLine(" ");
        Console.WriteLine("Enter a new 5 letter word");
        newWord = Console.ReadLine();
    }
    else
    {
        newBroken = newWord.ToCharArray();

        for (int i = 0; i < 5; i++)
        {
            if (word.Contains(newBroken[i]))
            {
                if (brokenWord[i] == newBroken[i])
                {
                    correct[i] = newBroken[i];
                }
                else
                {
                    wrongPlace.Add(newBroken[i]);
                }
            }
            else
            {
                bad.Add(newBroken[i]);
            }
        }

        Console.Write("Correct letters on correct place: ");
        for (int i = 0; i < correct.Length; i++)
        {
            Console.Write(correct[i] + " ");
        }

        Console.WriteLine(" ");
        Console.Write("Correct letters but worng place: ");
        for (int i = 0; i < wrongPlace.Count; i++)
        {
            Console.Write(wrongPlace[i] + " ");
        }

        Console.WriteLine(" ");
        Console.Write("Wrong letters: ");
        for (int i = 0; i < bad.Count; i++)
        {
            Console.Write(bad[i] + " ");
        }
    }
    counter++;
}
Console.WriteLine("Poggies you got it right!");
