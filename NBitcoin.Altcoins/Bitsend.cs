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
	public class Bitsend : NetworkSetBase
	{
		public static Bitsend Instance { get; } = new Bitsend();

		public override string CryptoCode => "BSD";

		private Bitsend()
		{

		}
		//Format visual studio
		//{({.*?}), (.*?)}
		//Tuple.Create(new byte[]$1, $2)
		static Tuple<byte[], int>[] pnSeed6_main = {
			//seed.mybitsend.com, 104.161.113.89
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x68,0xa1,0x71,0x59}, 8886),
			//37.120.186.85
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x25,0x78,0xba,0x55}, 8886),
			//37.120.190.76
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x25,0x78,0xbe,0x4c}, 8886),
			//213.136.80.93
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xd5,0x88,0x50,0x5d}, 8886),
			//213.136.86.202
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xd5,0x88,0x56,0xca}, 8886),
			//213.136.86.205
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xd5,0x88,0x56,0xcd}, 8886),
			//213.136.86.207
                        Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xd5,0x88,0x56,0xcf}, 8886),
			//188.68.52.172
                        Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xbc,0x44,0x34,0xac}, 8886),
		};
		static Tuple<byte[], int>[] pnSeed6_test = { };

#pragma warning disable CS0618 // Type or member is obsolete
		public class BitsendConsensusFactory : ConsensusFactory
		{
			private BitsendConsensusFactory()
			{
			}

			public static BitsendConsensusFactory Instance { get; } = new BitsendConsensusFactory();

			public override BlockHeader CreateBlockHeader()
			{
				return new BitsendBlockHeader();
			}
			public override Block CreateBlock()
			{
				return new BitsendBlock(new BitsendBlockHeader());
			}
		}

		public class BitsendBlockHeader : BlockHeader
		{
			public override uint256 GetPoWHash()
			{
				//BSD Algo implement here
                                throw new NotSupportedException("PoW for BitSend BSD is not supported");
			}
		}

		public class BitsendBlock : Block
		{
			public BitsendBlock(BitsendBlockHeader header) : base(header)
			{

			}
			public override ConsensusFactory GetConsensusFactory()
			{
				return BitsendConsensusFactory.Instance;
			}
		}

#pragma warning restore CS0618 // Type or member is obsolete

		protected override void PostInit()
		{
			//RegisterDefaultCookiePath("Bitsend", new FolderName() { TestnetFolder = "testnet3" });
			RegisterDefaultCookiePath(Mainnet, ".cookie");
                        RegisterDefaultCookiePath(Testnet, "testnet3", ".cookie");
                        RegisterDefaultCookiePath(Regtest, "regtest", ".cookie");
		}

		protected override NetworkBuilder CreateMainnet()
		{
			NetworkBuilder builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000, 
				MajorityEnforceBlockUpgrade = 750, //LTC
				MajorityRejectBlockOutdated = 950, //LTC
				MajorityWindow = 1000, //LTC
				BIP34Hash = new uint256("000000000000024b89b42a942fe0d9fea3bb44ab7bd1b19115dd6a759c0808b8"), 
				PowLimit = new Target(new uint256("000000000000024b89b42a942fe0d9fea3bb44ab7bd1b19115dd6a759c0808b8")),
				PowTargetTimespan = TimeSpan.FromSeconds(6 * 24 * 60 * 60), //two weeks
				PowTargetSpacing = TimeSpan.FromSeconds(5 * 60),
				PowAllowMinDifficultyBlocks = false,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 1,
				MinerConfirmationWindow = 1728,
				CoinbaseMaturity = 100, //
				ConsensusFactory = BitsendConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 102 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 5 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 204 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x88, 0xB2, 0x1E })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x88, 0xAD, 0xE4 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("bsd"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("bsd"))
			.SetMagic(0xF9C2D5A3) //defined in inverted direction, 0xA3D5C2F9
			.SetPort(8886) //P2P port 
			.SetRPCPort(8800)
			.SetMaxP2PVersion(70084)
			.SetName("bsd-main")
			.AddAlias("bsd-mainnet")
			.AddAlias("bitsend-mainnet")
			.AddAlias("bitsend-main")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("seed.mybitsend.com", "seed.mybitsend.com"),
				new DNSSeedData("37.120.186.85", "37.120.186.85"),
				new DNSSeedData("37.120.190.76", "37.120.190.76"),
				new DNSSeedData("213.136.80.93", "213.136.80.93"),
				new DNSSeedData("213.136.86.202", "213.136.86.202"),
				new DNSSeedData("213.136.86.205", "213.136.86.205"),
				new DNSSeedData("213.136.86.207", "213.136.86.207"),
				new DNSSeedData("188.68.52.172", "188.68.52.172")
			})
			.AddSeeds(ToSeed(pnSeed6_main)) 
			.SetGenesis("01000000000000000000000000000000000000000000000000000000000000000000000057df57a347a2e5cf9b6973bbdf7a8585a0b595a8a8d7c7b6318cb79489f6c4c03d4f9253f0ff0f1e5fc412020101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4304ffff001d01043b6c696d65636f696e58206f667265636520616d706c69612067616d6120646520736572766963696f732079206d656a6f7261732070617261207469ffffffff0100e40b54020000004341040184710fa689ad5023690c80f3a49c8f13f8d45b8c857fbcbc8bc4a8e4d3eb4b10f4d4604fa08dce601aaf0f470216fe1b51850b4acf21b179c45070ac7b03a9ac00000000"); 
			return builder;
		}

   		protected override NetworkBuilder CreateTestnet()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000,
				MajorityEnforceBlockUpgrade = 51, //
				MajorityRejectBlockOutdated = 75, //
				MajorityWindow = 1000, //
				PowLimit = new Target(new uint256("0x0000012e1b8843ac9ce8c18603658eaf8895f99d3f5e7e1b7b1686f35e3c087a")),
				PowTargetTimespan = TimeSpan.FromSeconds(3.5 * 24 * 60 * 60),
				PowTargetSpacing = TimeSpan.FromSeconds(2.5 * 60),
				PowAllowMinDifficultyBlocks = true,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 1,
				MinerConfirmationWindow = 2,
				CoinbaseMaturity = 100, //
				ConsensusFactory = BitsendConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 111 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 196 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x35, 0x87, 0xCF })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x35, 0x83, 0x94 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tbsd"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tbsd"))
			.SetMagic(0xF1C8D2FD) //defined in inverted direction, 0xFDD2C8F1
			.SetPort(8666) // P2P port
			.SetRPCPort(18800)
			.SetMaxP2PVersion(70084)
			.SetName("bsd-test")
			.AddAlias("bsd-testnet")
			.AddAlias("bitsend-test")
			.AddAlias("bitsend-testnet")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("testnetbitsend.jonasschnelli.ch", "testnet-seed.bitsend.jonasschnelli.ch"),
				new DNSSeedData("petertodd.org", "seed.tbtc.petertodd.org"),
				new DNSSeedData("bluematt.me", "testnet-seed.bluematt.me"),
				new DNSSeedData("bitsend.schildbach.de", "testnet-seed.bitsend.schildbach.de")
			})
			.AddSeeds(ToSeed(pnSeed6_test))
			.SetGenesis("01000000000000000000000000000000000000000000000000000000000000000000000057df57a347a2e5cf9b6973bbdf7a8585a0b595a8a8d7c7b6318cb79489f6c4c03d4f9253f0ff0f1e5fc412020101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4304ffff001d01043b6c696d65636f696e58206f667265636520616d706c69612067616d6120646520736572766963696f732079206d656a6f7261732070617261207469ffffffff0100e40b54020000004341040184710fa689ad5023690c80f3a49c8f13f8d45b8c857fbcbc8bc4a8e4d3eb4b10f4d4604fa08dce601aaf0f470216fe1b51850b4acf21b179c45070ac7b03a9ac00000000");
			return builder;
		}

		protected override NetworkBuilder CreateRegtest()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 150,
				MajorityEnforceBlockUpgrade = 51, //
				MajorityRejectBlockOutdated = 75, //
				MajorityWindow = 144, //
				PowLimit = new Target(new uint256("7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = TimeSpan.FromSeconds(14 * 24 * 60 * 60),
				PowTargetSpacing = TimeSpan.FromSeconds(10 * 60),
				PowAllowMinDifficultyBlocks = true,
				MinimumChainWork = uint256.Zero,
				PowNoRetargeting = true,
				RuleChangeActivationThreshold = 108,
				MinerConfirmationWindow = 144,
				CoinbaseMaturity = 100, //
				ConsensusFactory = BitsendConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 111 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 196 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x35, 0x87, 0xCF })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x35, 0x83, 0x94 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("tbsd")) 
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("tbsd")) 
			.SetMagic(0xAF2C1A4C) //defined in inverted direction, 0x4C1A2CAF
			.SetPort(8885) // P2P port
			.SetRPCPort(18332)
			.SetMaxP2PVersion(70084)
			.SetName("bsd-reg")
			.AddAlias("bsd-regtest")
			.AddAlias("bitsend-reg")
			.AddAlias("bitsend-regtest")
			.SetGenesis("01000000000000000000000000000000000000000000000000000000000000000000000057df57a347a2e5cf9b6973bbdf7a8585a0b595a8a8d7c7b6318cb79489f6c4c03d4f9253f0ff0f1e5fc412020101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4304ffff001d01043b6c696d65636f696e58206f667265636520616d706c69612067616d6120646520736572766963696f732079206d656a6f7261732070617261207469ffffffff0100e40b54020000004341040184710fa689ad5023690c80f3a49c8f13f8d45b8c857fbcbc8bc4a8e4d3eb4b10f4d4604fa08dce601aaf0f470216fe1b51850b4acf21b179c45070ac7b03a9ac00000000");
			return builder;
		}
	}
}
