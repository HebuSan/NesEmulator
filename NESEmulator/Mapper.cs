﻿using System;
using log4net;

namespace NESEmulator
{
    public abstract class Mapper
    {
        protected ILog Log = LogManager.GetLogger(typeof(Mapper));

        protected ulong clockCycle;
        protected ulong cpuClockCycle;

        protected byte nPRGBanks;
        protected byte nCHRBanks;
        protected readonly Cartridge cartridge;

        public abstract bool HasBusConflicts { get; }
        public abstract bool PRGRAMEnable { get; protected set; }
        public virtual bool HasScanlineCounter { get => false; }

        public Mapper(Cartridge cart, byte prgBanks, byte chrBanks)
        {
            cartridge = cart;
            nPRGBanks = prgBanks;
            nCHRBanks = chrBanks;
        }

        // Transform CPU bus address into PRG ROM offset
        public abstract bool cpuMapRead(ushort addr, out uint mapped_addr, ref byte data);
        public abstract bool cpuMapWrite(ushort addr, out uint mapped_addr, ref byte data);

        // Transform PPU bus address into CHR ROM offset
        public abstract bool ppuMapRead(ushort addr, out uint mapped_addr, ref byte data);
        public abstract bool ppuMapWrite(ushort addr, out uint mapped_addr, ref byte data);

        public abstract void reset();

        // for debugging
        public virtual void clock(ulong clockCycle)
        {
            this.clockCycle = clockCycle;
            if ((clockCycle & 0x3) == 0)
                ++cpuClockCycle;
        }
    }
}
