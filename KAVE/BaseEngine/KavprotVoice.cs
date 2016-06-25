using System;
using System.Collections.Generic;
using System.Text;
using System.Speech.Synthesis;
using KAVE.MMedia.Mp3;
using WaveLib;
using System.IO;
using System.Speech.Recognition;
using System.Globalization;
using System.Speech.AudioFormat;
using System.Reflection;

namespace KAVE.BaseEngine
{
   public static class KavprotVoice
   {

       public static SpeechSynthesizer SPS;

       public static void Initialize()
       {
           try
           {
               SPS = new SpeechSynthesizer();
            
               SPS.Rate = 0;
             
               Prompts = new List<Prompt>();

               Initialized = true;

           }
           catch (Exception ex)
           {
               Initialized = false;
             
           }
           finally
           {

           }

       }

  
       public static bool Initialized = false;
       public static void Speak(string text)
       {
           try
           {
               if (Initialized)
               {
                   SPS.Speak(text);
               }
               else
               {

                   Initialize();
                   SPS.Speak(text);
               }


           }
           catch (Exception ex)
           {
              
           }
           finally
           {

           }
       }
       public static List<Prompt> Prompts;

       public static void SpeakAsync(string text)
       {
           try
           {

               if (Initialized)
               {

                   Prompts.Add(SPS.SpeakAsync(text));
               }
               else
               {
                   Initialize();
                   Prompts.Add(SPS.SpeakAsync(text));


               }

           }
           catch (Exception ex)
           {
           }
           finally
           {

           }
       }
       public static void SpeakInWave(string text, string wavefile)
       {
           try
           {

               if (Initialized)
               {
         
                   var fmt = new SpeechAudioFormatInfo(8000, AudioBitsPerSample.Eight, AudioChannel.Mono);
                   SPS.SetOutputToWaveFile(wavefile,fmt );
                   SPS.Speak(text);
                   SPS.SetOutputToDefaultAudioDevice();
                 }
               else
               {
                   Initialize();
                   var fmt = new SpeechAudioFormatInfo(8000, AudioBitsPerSample.Eight, AudioChannel.Mono);
                   SPS.SetOutputToWaveFile(wavefile, fmt);
                   SPS.Speak(text);
                   SPS.SetOutputToDefaultAudioDevice();
               }


           }
           catch (Exception ex)
           {
            
           }
           finally
           {

           }
       }
       public static void StopTalking()
       {
           try
           {

               SPS.SpeakAsyncCancelAll();
           }
           catch
           {

           }
           finally
           {

           }

       }
       public static void ConvertWavToMp3(string wav, string mp3)
       {
           Mp3WriterConfig m_Config = null;
           WaveStream InStr = new WaveStream(wav);
           m_Config = new Mp3WriterConfig(InStr.Format);
           m_Config.Mp3Config.format.mp3.bOriginal = 80;

           try
           {

               Mp3Writer writer = new Mp3Writer(new FileStream(mp3, FileMode.Create), m_Config);

               try
               {
                   byte[] buff = new byte[writer.OptimalBufferSize];
                   int read = 0;
                   int actual = 0;
                   long total = InStr.Length;

                   try
                   {
                       while ((read = InStr.Read(buff, 0, buff.Length)) > 0)
                       {

                           writer.Write(buff, 0, read);
                           actual += read;


                       }

                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine(ex.Message);
                   }
                   finally
                   {

                   }


               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex.Message);
               }
               finally
               {
                   writer.Close();
               }
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message);
           }
           finally
           {
               InStr.Close();
           }
       }
    }
}
