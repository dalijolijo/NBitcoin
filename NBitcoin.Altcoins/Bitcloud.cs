using NBitcoin;
using NBitcoin.DataEncoders;
using NBitcoin.Protocol;
using NBitcoin.RPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace NBitcoin.Altcoins
{
	public class Bitcloud : NetworkSetBase
	{
		public static Bitcloud Instance { get; } = new Bitcloud();

		public override string CryptoCode => "BTDX";

		private Bitcloud()
		{

		}
		//Format visual studio
		//{({.*?}), (.*?)}
		//Tuple.Create(new byte[]$1, $2)
		static Tuple<byte[], int>[] pnSeed6_main = {
			//188.68.52.172
                        Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xbc,0x44,0x34,0xac}, 8329),
                        //37.120.186.85
                        Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x25,0x78,0xba,0x55}, 8329),
                        //37.120.190.76
                        Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x25,0x78,0xbe,0x4c}, 8329),
		};
		static Tuple<byte[], int>[] pnSeed6_test = { };

#pragma warning disable CS0618 // Type or member is obsolete
		public class BitcloudConsensusFactory : ConsensusFactory
		{
			private BitcloudConsensusFactory()
			{
			}

			public static BitcloudConsensusFactory Instance { get; } = new BitcloudConsensusFactory();

			public override BlockHeader CreateBlockHeader()
			{
				return new BitcloudBlockHeader();
			}
			public override Block CreateBlock()
			{
				return new BitcloudBlock(new BitcloudBlockHeader());
			}
		}

		public class BitcloudBlockHeader : BlockHeader
		{
			public override uint256 GetPoWHash()
			{
				//BTDX Algo implement here
                                throw new NotSupportedException("PoW for Bitcloud BTDX is not supported");
			}
		}

		public class BitcloudBlock : Block
		{
			public BitcloudBlock(BitcloudBlockHeader header) : base(header)
			{

			}
			public override ConsensusFactory GetConsensusFactory()
			{
				return BitcloudConsensusFactory.Instance;
			}
		}

#pragma warning restore CS0618 // Type or member is obsolete

		protected override void PostInit()
		{
			RegisterDefaultCookiePath("Bitcloud");
		}

		protected override NetworkBuilder CreateMainnet()
		{
			NetworkBuilder builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000, 
				MajorityEnforceBlockUpgrade = 750,
				MajorityRejectBlockOutdated = 950,
				MajorityWindow = 1000, //TODO nToCheckBlockUpgradeMajority = 1000
				BIP34Hash = new uint256("000000000000024b89b42a942fe0d9fea3bb44ab7bd1b19115dd6a759c0808b8"), //TODO
				PowLimit = new Target(new uint256("0000000000000000000000000000000000000000000000000000000000000000")), 
				PowTargetTimespan = TimeSpan.FromSeconds(1 * 300),
				PowTargetSpacing = TimeSpan.FromSeconds(1 * 300),
				PowAllowMinDifficultyBlocks = true,
				PowNoRetargeting = false, //TODO
				RuleChangeActivationThreshold = 1, //TODO
				MinerConfirmationWindow = 1728, //TODO
				CoinbaseMaturity = 144, //TODO nMaturity =144;
				ConsensusFactory = BitcloudConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 25 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 5 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 153 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x88, 0xB2, 0x1E })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x88, 0xAD, 0xE4 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("btdx"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("btdx"))
			.SetMagic(0xFDBDE8E4) //defined in inverted direction, 0xE4E8BDFD
			.SetPort(8329) //P2P port 
			.SetRPCPort(8330)
			.SetMaxP2PVersion(70714)
			.SetName("btdx-main")
			.AddAlias("btdx-mainnet")
			.AddAlias("bitcloud-mainnet")
			.AddAlias("bitcloud-main")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("seed.bitcloud.network", "seed.bitcloud.network"),
				new DNSSeedData("188.68.52.172", "188.68.52.172"),
				new DNSSeedData("37.120.186.85", "37.120.186.85"),
				new DNSSeedData("37.120.190.76", "37.120.190.76")
			})
			.AddSeeds(ToSeed(pnSeed6_main)) 
			.SetGenesis("010000000000000000000000000000000000000000000000000000000000000000000000cb070ee1e17311c4caecfd9f4e649322955f9c79e7ae70fa4643eab10f8ef368102c9359f0ff0f1e45410e000101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4904ffff001d0104415468652074696d65732033312e30382e3230313620426974636c6f75642077617320666f756e646564202d2044657369676e6564205465616d2042697473656e64ffffffff010000000000000000434104678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5fac00000000"); //TODO assert(hashGenesisBlock == uint256("0x000002d56463941c20eae5cb474cc805b646515d18bc7dc222a0885b206eadb0"));
			return builder;
		}

   		protected override NetworkBuilder CreateTestnet()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000,
				MajorityEnforceBlockUpgrade = 51,
				MajorityRejectBlockOutdated = 75,
				MajorityWindow = 100, //TODO nToCheckBlockUpgradeMajority = 100;
				PowLimit = new Target(new uint256("0000000000000000000000000000000000000000000000000000000000000000")), //TODO not defined
				PowTargetTimespan = TimeSpan.FromSeconds(1 * 135),
				PowTargetSpacing = TimeSpan.FromSeconds(1 * 135),
				PowAllowMinDifficultyBlocks = true,
				PowNoRetargeting = false, //TODO
				RuleChangeActivationThreshold = 1, //TODO
				MinerConfirmationWindow = 2, //TODO
				CoinbaseMaturity = 15, //TODO nMaturity =15;
				ConsensusFactory = BitcloudConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 139 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 19 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x3A, 0x80, 0x61, 0xA0 })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x3A, 0x80, 0x58, 0x37 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tbtdx"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tbtdx"))
			.SetMagic(0xBA657645) //defined in inverted direction, 0x457665BA
			.SetPort(51474) // P2P port
			.SetRPCPort(8329)
			.SetMaxP2PVersion(70084)
			.SetName("btdx-test")
			.AddAlias("btdx-testnet")
			.AddAlias("bitcloud-test")
			.AddAlias("bitcloud-testnet")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("188.68.52.172", "188.68.52.172"),
				new DNSSeedData("37.120.186.85", "37.120.186.85"),
				new DNSSeedData("37.120.190.76", "37.120.190.76")
			})
			.AddSeeds(ToSeed(pnSeed6_test))
			.SetGenesis("010000000000000000000000000000000000000000000000000000000000000000000000cb070ee1e17311c4caecfd9f4e649322955f9c79e7ae70fa4643eab10f8ef368102c9359f0ff0f1e45410e000101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4904ffff001d0104415468652074696d65732033312e30382e3230313620426974636c6f75642077617320666f756e646564202d2044657369676e6564205465616d2042697473656e64ffffffff010000000000000000434104678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5fac00000000"); //TODO same as main?
			return builder;
		}

		protected override NetworkBuilder CreateRegtest()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 150,
				MajorityEnforceBlockUpgrade = 750,
				MajorityRejectBlockOutdated = 950,
				MajorityWindow = 1000, //TODO
				PowLimit = new Target(new uint256("0000000000000000000000000000000000000000000000000000000000000000")),
				PowTargetTimespan = TimeSpan.FromSeconds(24 * 60 * 60),
				PowTargetSpacing = TimeSpan.FromSeconds(1 * 60),
				PowAllowMinDifficultyBlocks = true,
				MinimumChainWork = uint256.Zero,
				PowNoRetargeting = true, //TODO
				RuleChangeActivationThreshold = 108, //TODO
				MinerConfirmationWindow = 144, //TODO
				CoinbaseMaturity = 15, //TODO
				ConsensusFactory = BitcloudConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 139 }) //TODO
                        .SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 19 }) //TODO
                        .SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 }) //TODO
                        .SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x3A, 0x80, 0x61, 0xA0 }) //TODO
                        .SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x3A, 0x80, 0x58, 0x37 }) //TODO
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tbtdx")) 
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tbtdx")) 
			.SetMagic(0xAC7ECFA1) //defined in inverted direction, 0xA1CF7EAC
			.SetPort(51476) // P2P port
			.SetRPCPort(8329) //TODO
			.SetMaxP2PVersion(70084)
			.SetName("btdx-reg")
			.AddAlias("btdx-regtest")
			.AddAlias("bitcloud-reg")
			.AddAlias("bitcloud-regtest")
			.SetGenesis("010000000000000000000000000000000000000000000000000000000000000000000000cb070ee1e17311c4caecfd9f4e649322955f9c79e7ae70fa4643eab10f8ef368102c9359f0ff0f1e45410e000101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4904ffff001d0104415468652074696d65732033312e30382e3230313620426974636c6f75642077617320666f756e646564202d2044657369676e6564205465616d2042697473656e64ffffffff010000000000000000434104678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5fac00000000"); //TODO same as main?
			return builder;
		}
	}
}
