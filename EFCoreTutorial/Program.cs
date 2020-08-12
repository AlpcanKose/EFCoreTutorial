using EFCoreTutorial.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCoreTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            TestVerisiOlustur(100, 300);
            SehirBazlıAnalizYap();
            Console.ReadLine();
        }
        
        private static void TestVerisiOlustur(int musteriAdet, int sepetAdet)
        {
            string[] cities = new string[] { "Ankara", "İstanbul", "İzmir", "Bursa", "Edirne", "Konya", "Antalya", "Diyarbakır", "Van", "Rize" };
            string[] names = new string[] { "Alp", "Ali", "Hasan", "Mustafa", "Kerem", "Taner", "Mikail" };
            string[] surnames = new string[] { "Köse", "Kaldırım", "Karaca", "Açık", "Günay", "Eser", "Güler" };
            int[] currentIDs = new int[musteriAdet];
            Random rnd = new Random();
            for (int i = 0; i < musteriAdet; i++)
            {
                int cityIndex = rnd.Next(0, 9);
                int nameIndex = rnd.Next(0, 6);
                int surnameIndex = rnd.Next(0, 6);
                using (var context = new EFCoreTutorialContext())
                {
                    Musteri musteri = new Musteri()
                    {
                        Ad = names[nameIndex],
                        Soyad = surnames[surnameIndex],
                        Sehir = cities[cityIndex]
                    };
                    context.Add(musteri);
                    context.SaveChanges();
                }
                using (var context = new EFCoreTutorialContext())
                {
                    var musteri = context.Musteri.OrderByDescending(x=>x.Id).FirstOrDefault();
                    currentIDs[i] = musteri.Id;
                }

            }
            for (int i = 0; i < sepetAdet; i++)
            {
                int musteriID = rnd.Next(0, musteriAdet - 1);
                int sepetID;
                using (var context = new EFCoreTutorialContext())
                {
                    Sepet sepet = new Sepet()
                    {
                        MusteriId = currentIDs[musteriID],
                        Tarih = DateTime.Now
                    };
                    context.Add(sepet);
                    context.SaveChanges();
                }
                using (var context = new EFCoreTutorialContext())
                {
                    var sepet = context.Sepet.OrderByDescending(x => x.Id).FirstOrDefault();
                    sepetID = sepet.Id;
                    int sepetCount = rnd.Next(1, 5);
                    for (int j = 0; j < sepetCount; j++)
                    {
                        SepetUrun sepetUrun = new SepetUrun()
                        {
                            SepetId = sepetID,
                            Tutar = rnd.Next(100, 1000),
                            Aciklama = "Açıklama"
                        };
                        context.Add(sepetUrun);
                        context.SaveChanges();
                    }
                }
            }
        }

        private static void SehirBazlıAnalizYap()
        {
            string[] cities = new string[] { "Ankara", "İstanbul", "İzmir", "Bursa", "Edirne", "Konya", "Antalya", "Diyarbakır", "Van", "Rize" };
            int toplamTutar;
            int sepetSayisi=0;
            List<Result> results = new List<Result>();
            for (int i = 0; i < cities.Length; i++)
            {
                toplamTutar = 0;
                sepetSayisi = 0;
                using (var context = new EFCoreTutorialContext())
                {
                    var musteriler = context.Musteri.Where(x => x.Sehir == cities[i]).ToList();
                    foreach(var musteri in musteriler)
                    {
                        var sepetler = context.Sepet.Where(x => x.MusteriId == musteri.Id).ToList();
                        sepetSayisi+= sepetler.Count();
                        foreach (var sepet in sepetler)
                        {
                            var sepetUrunleri = context.SepetUrun.Where(x => x.SepetId == sepet.Id).ToList();
                            foreach (var sepetUrun in sepetUrunleri)
                            {
                                toplamTutar +=  (int)sepetUrun.Tutar;
                            }
                        }
                    }
                    Result result = new Result()
                    {
                        City = cities[i],
                        Count = sepetSayisi,
                        Price = toplamTutar
                    };
                    results.Add(result);
                }
            }
            results = results.OrderByDescending(res => res.Count).ToList();
            foreach (var result in results)
            {
                Console.WriteLine(result.City + " - " + result.Count + " - " + result.Price + " TL");
            }

            Console.WriteLine("Toplam Sepet Adedi: " + results.Sum(res => res.Count));
            Console.WriteLine("Toplam Tutar: " + results.Sum(res => res.Price));
        }

    }
}
