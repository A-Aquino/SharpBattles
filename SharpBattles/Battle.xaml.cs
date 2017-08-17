using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharpBattles
{
    /// <summary>
    /// Interaction logic for Battle.xaml
    /// </summary>
    public partial class Battle : Page
    {

        string p1;
        string p2;
        string[] p1Attacks;
        string[] p2Attacks;
        int p1Health;
        int p2Health;
        int p1Defend;
        int p2Defend;

        Random random;

        public Battle(string a, string b)
        {
            InitializeComponent();
            p1 = a;
            p2 = b;
            p1Attacks = getAttacks(p1);
            p2Attacks = getAttacks(p2);
            p1Health = 1000;
            p2Health = 1000;
            p1Defend = 0;
            p2Defend = 0;

            random = new Random();

            //Prepare layout
            _vsText.Text = p1 + " vs. " + p2;
            _player1.Source = new BitmapImage(new Uri(@"Images/" + p1 + ".png", UriKind.Relative));
            _player2.Source = new BitmapImage(new Uri(@"Images/" + p2 + ".png", UriKind.Relative));
            _player1Name.Text = p1;
            _player2Name.Text = p2;
            _battleNotes.Text = "A wild " + p2 + " appears!";

            //Make attacks
            makeAttacks();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Attacks().Show();
        }

        //Attack List
        private string[] getAttacks(string player)
        {
            switch (player)
            {
                //Player Classes
                case "Swordsman": return new string[] { "Slash", "Stab", "Dodge" };
                case "Archer": return new string[] { "Shoot", "Defend", "Dodge" };
                case "Mage": return new string[] { "Fireball", "Thunder", "Heal" };
                case "Knight": return new string[] { "Stab", "Defend" };

                //Enemies
                case "Slime": return new string[] { "Slash", "Dodge", "Heal" };
                case "Goblin": return new string[] { "Slash", "Shoot", "Defend" };
                case "Bat": return new string[] { "Slash", "Dodge" };
                case "Mimic": return new string[] { "Fireball", "Thunder", "Dark Arts" };
                case "Witch": return new string[] { "Thunder", "Dark Arts", "Heal" };
                case "Dragon": return new string[] { "Stab", "Thunder", "Defend" };
                case "Wyvern": return new string[] { "Stab", "Dark Arts", "Defend" };
                default: return null;
            }
        }

        //Make Attack Buttons
        private void makeAttacks()
        {
            foreach (string s in p1Attacks)
                makeButton(s);
        }

        //Update Player Health
        private string updateP1Health(int v)
        {
            string t = "";
            if (v < 0)
            {
                if (p1Defend == 1) { v /= 4; }
                if (p1Defend == 2) { if (random.Next(0, 100) > 50) v = 0; }
                t = "Dealt " + Math.Abs(v) + " damage.";
                if (p1Defend == 1) t += " " + p1 + " defended the attack!";
                if (p1Defend == 2 && v != 0) t += " " + p1 + " attempted to dodge, but failed!";
                if (p1Defend == 2 && v == 0) t += " " + p1 + " successfully dodged the attack!";
                p1Defend = 0;
            }
            p1Health += v;
            if (p1Health < 0) p1Health = 0;
            if (p1Health > 1000) p1Health = 1000;
            _player1Health.Text = "" + p1Health;
            return t;
        }

        //Update Enemy Health
        private string updateP2Health(int v)
        {
            string t = "";
            if (v < 0)
            {
                if (p2Defend == 1) { v /= 4; }
                if (p2Defend == 2) { if (random.Next(0, 100) > 50) v = 0; }
                t = "Dealt " + Math.Abs(v) + " damage.";
                if (p2Defend == 1) t += " " + p2 + " defended the attack!";
                if (p2Defend == 2 && v != 0) t += " " + p2 + " attempted to dodge, but failed!";
                if (p2Defend == 2 && v == 0) t += " " + p2 + " successfully dodged the attack!";
                p2Defend = 0;
            }
            p2Health += v;
            if (p2Health < 0) p2Health = 0;
            if (p2Health > 1000) p2Health = 1000;
            _player2Health.Text = "" + p2Health;
            return t;
        }

        //Pick a random enemy move
        private string pickEnemyMove()
        {
            return p2Attacks[random.Next(0, p2Attacks.Length)];
        }

        //Damage randomiser
        private int damageCalc(int dmg, int rnd)
        {
            return -dmg + random.Next(0, rnd*2) - rnd;
        }

        //Handle all button presses
        private void buttonPress(object sender, RoutedEventArgs e)
        {
            //Remove Buttons
            _attackList.Children.Clear();

            //Button Text
            Button b = (Button)sender;
            string t = (string)b.Content;

            //Check End State
	        if(t=="Battle End"||t=="Game Over") {
		        this.NavigationService.GoBack();
		        return;
	        }
	
	        //Check Health
	        if(p2Health==0) {
		        _battleNotes.Text = p1+" wins the battle!";
		        makeButton("Battle End");
		        return;
	        }
	        if(p1Health==0) {
		        _battleNotes.Text = p1+" died!";
		        makeButton("Game Over");
		        return;
	        }
	
	        //Continue
	        if(t=="Continue") {
		
		        //Pick enemy moves
		        t = pickEnemyMove();

		        //Smarter AI
		        while( (p2Defend>0&&(t=="Dodge"||t=="Defend")) || (p2Health==1000&&t=="Heal") )
			        t = pickEnemyMove();
		
		        //Enemy moves
		        if(t=="Slash") {
                    _battleNotes.Text = p2 + " used Slash. " + updateP1Health(damageCalc(200,10));
		        }
		        if(t=="Stab") {
                    if (random.Next(0, 100) > 50)
                        _battleNotes.Text = p2 + " used Stab. " + updateP1Health(damageCalc(400, 15));
			        else
                        _battleNotes.Text = p2 + " used Stab, and missed.";
		        }
		        if(t=="Shoot") {
                    if (random.Next(0, 100) > 25)
                        _battleNotes.Text = p2 + " used Shoot. " + updateP1Health(damageCalc(300, 15));
			        else
                        _battleNotes.Text = p2 + " used Shoot, and missed.";
		        }
		        if(t=="Dodge") {
                    _battleNotes.Text = p2 + " prepares to dodge an attack.";
                    p2Defend = 2;
		        }
		        if(t=="Defend") {
                    _battleNotes.Text = p2 + " prepares to defend an attack.";
                    p2Defend = 1;
		        }
		        if(t=="Fireball") {
                    if (random.Next(0, 100) > 25)
                        _battleNotes.Text = p2 + " used Fireball. " + updateP1Health(damageCalc(300, 15));
			        else
                        _battleNotes.Text = p2 + " used Fireball, and missed.";
		        }
		        if(t=="Thunder") {
                    if (random.Next(0, 100) > 75)
                        _battleNotes.Text = p2 + " used Thunder. " + updateP1Health(damageCalc(500, 20));
			        else
                        _battleNotes.Text = p2 + " used Thunder, and missed.";
		        }
		        if(t=="Heal") {
                    int heal = -damageCalc(175, 75);
                    _battleNotes.Text = p2 + " used Heal, and recovered " + heal + " HP!";
                    updateP2Health(heal);
		        }
		        if(t=="Dark Arts") {
                    if (random.Next(0, 100) > 85)
                        _battleNotes.Text = p2 + " used Dark Arts. " + updateP1Health(damageCalc(800, 150));
			        else
                        _battleNotes.Text = p2 + " used Dark Arts, and missed.";
		        }
                if (p1Health > 0)
			        makeAttacks();
		        else
			        makeButton("Continue");
	        }
	
	        //Player moves
	        else {
		        if(t=="Slash") {
                    _battleNotes.Text = p1 + " used Slash. " + updateP2Health(damageCalc(200, 10));
		        }
		        if(t=="Stab") {
                    if (random.Next(0, 100) > 50)
                        _battleNotes.Text = p1 + " used Stab. " + updateP2Health(damageCalc(400, 15));
			        else
                        _battleNotes.Text = p1 + " used Stab, and missed.";
		        }
		        if(t=="Shoot") {
                    if (random.Next(0, 100) > 25)
                        _battleNotes.Text = p1 + " used Shoot. " + updateP2Health(damageCalc(300, 15));
			        else
                        _battleNotes.Text = p1 + " used Shoot, and missed.";
		        }
		        if(t=="Dodge") {
                    _battleNotes.Text = p1 + " prepares to dodge an attack.";
                    p1Defend = 2;
		        }
		        if(t=="Defend") {
                    _battleNotes.Text = p1 + " prepares to defend an attack.";
                    p1Defend = 1;
		        }
		        if(t=="Fireball") {
                    if (random.Next(0, 100) > 25)
                        _battleNotes.Text = p1 + " used Fireball. " + updateP2Health(damageCalc(300, 15));
			        else
                        _battleNotes.Text = p1 + " used Fireball, and missed.";
		        }
		        if(t=="Thunder") {
                    if (random.Next(0, 100) > 75)
                        _battleNotes.Text = p1 + " used Thunder. " + updateP2Health(damageCalc(500, 20));
			        else
                        _battleNotes.Text = p1 + " used Thunder, and missed.";
		        }
		        if(t=="Heal") {
                    int heal = -damageCalc(175, 75);
                    _battleNotes.Text = p1 + " used Heal, and recovered " + heal + " HP!";
                    updateP1Health(heal);
		        }
		        makeButton("Continue");
        }
        }

        //Make buttons
        private void makeButton(string t)
        {
            Button b = new Button();
            b.Height = 32;
            b.FontSize = 18;
            b.Click += buttonPress;
            b.Content = t;
            _attackList.Children.Add(b);
        }


    }
}
