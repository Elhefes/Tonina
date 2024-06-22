using System;
using UnityEngine;

public class AudioLooper : MonoBehaviour
{
    //Credit: https://discussions.unity.com/u/Untherow/summary

    /// <summary>
    /// Trims silence from both ends in an AudioClip.
    /// Makes mp3 files seamlessly loopable.
    /// </summary>
    /// <param name="inputAudio"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public AudioClip trimSilence(AudioClip inputAudio, float threshold = 0.05f)
    {
        // Copy samples from input audio to an array. AudioClip uses interleaved format so the length in samples is multiplied by channel count
        float[] samplesOriginal = new float[inputAudio.samples * inputAudio.channels];
        inputAudio.GetData(samplesOriginal, 0);

        int channelCount = inputAudio.channels;
        int sampleCount = inputAudio.samples;

        // Find first and last sample (from any channel) that exceed the threshold
        int audioStart = -1;
        int audioEnd = -1;

        for (int i = 0; i < sampleCount; i++)
        {
            for (int j = 0; j < channelCount; j++)
            {
                if (samplesOriginal[i * channelCount + j] > threshold)
                {
                    if (audioStart == -1) audioStart = i;
                    audioEnd = i;
                }
            }
        }

        if (audioStart == -1 || audioEnd == -1)
        {
            // No samples exceed the threshold, return an empty AudioClip
            return AudioClip.Create(inputAudio.name, 0, inputAudio.channels, inputAudio.frequency, false);
        }

        // Calculate the number of samples to trim
        int trimmedLength = (audioEnd - audioStart + 1) * channelCount;
        float[] samplesTrimmed = new float[trimmedLength];

        // Copy trimmed audio data into another array while maintaining channel interleaving
        Array.Copy(samplesOriginal, audioStart * channelCount, samplesTrimmed, 0, trimmedLength);

        // Create new AudioClip for trimmed audio data
        AudioClip trimmedAudio = AudioClip.Create(inputAudio.name, trimmedLength / channelCount, inputAudio.channels, inputAudio.frequency, false);
        trimmedAudio.SetData(samplesTrimmed, 0);

        return trimmedAudio;
    }
}
