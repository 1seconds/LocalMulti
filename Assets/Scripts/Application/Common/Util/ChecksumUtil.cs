using UnityEngine;

public class Checksum {
    public int v1;
    public int v2;
    public int v3;
    public int v4;

    public long n => ((long)v2 << 32 | ((long)v1 & 0xFFFFFFFFL));
    public long h => ((long)v4 << 32 | ((long)v3 & 0xFFFFFFFFL));

    public Checksum() {
    }

    public Checksum(long n, long h) {
        v1 = (int)(n & 0xFFFFFFFFL);
        v2 = (int)((ulong)n >> 32);
        v3 = (int)(h & 0xFFFFFFFFL);
        v4 = (int)((ulong)h >> 32);
    }
}

public class ChecksumUtil {
    public static Checksum Build(long value) {
        var res = new Checksum();

        res.v1 = (int)(value >> 32);
        res.v2 = (int)(value & 0xFFFFFFFFL);
        res.v3 = Random.Range(0, 16);
        res.v1 = RotateLeft(res.v1, res.v3);
        res.v2 = RotateLeft(res.v2, res.v3);
        res.v4 = CalculateVn(value);
        res.v1 ^= res.v4;
        res.v2 ^= res.v4;
        res.v3 ^= res.v4;
        return res;
    }

    public static long Parse(Checksum checksum) {
        var v1 = checksum.v1 ^ checksum.v4;
        var v2 = checksum.v2 ^ checksum.v4;
        var v3 = checksum.v3 ^ checksum.v4;
        v1 = RotateRight(v1, v3);
        v2 = RotateRight(v2, v3);

        long value = (long)v1 << 32 | (long)v2;
        if (CalculateVn(value) != checksum.v4) {
            Debug.LogError("Hash Error!");
        }

        return value;
    }

    public static int CalculateVn(long v) {
        int hash = 0;
        while (v > 0) {
            int c = (int)(v & 0x7);
            v >>= 3;
            hash = c + (hash << 3) + (hash << 8) - hash;
        }

        return hash;
    }

    public static int RotateLeft(int value, int count) {
        uint val = (uint)value;
        return (int)((val << count) | (val >> (32 - count)));
    }

    public static int RotateRight(int value, int count) {
        uint val = (uint)value;
        return (int)((val >> count) | (val << (32 - count)));
    }
}