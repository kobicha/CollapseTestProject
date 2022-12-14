using System;
using System.Collections.Generic;
using System.Linq;
using Collapse.Blocks;
using UnityEngine;

namespace Collapse {
    /**
     * Partial class for separating the main functions that are needed to be modified in the context of this test
     */
    public partial class BoardManager
    {
        private static Vector2Int[] AdjecentDirections =
            { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        private static Vector2Int[] SurroundingDirections = {Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, 
            new Vector2Int(1,1),  new Vector2Int(-1,1),  new Vector2Int(1,-1),  new Vector2Int(-1,-1) };
        
        /**
         * Trigger a bomb
         */
        public void TriggerBomb(Bomb bomb) {
            var surroundingBlocks = GetSurroundingBlocks(bomb.GridPosition);
            foreach (var block in surroundingBlocks)
            {
                TriggerMatch(block);
            }
        }

        /**
         * Trigger a match
         */
        public void TriggerMatch(Block block) {
            if (block == null)
            {
                return;
            }
            // Find all blocks in this match
            var results = new List<Block>();
            var tested = new List<Vector2Int>();
            var blockPosition = new Vector2Int(block.GridPosition.x, block.GridPosition.y);
            FindChainRecursive(block.Type, blockPosition, tested, results);
            
            // Trigger blocks
            for (var i = 0; i < results.Count; i++)
            {
                var triggeredBlock = results[i];
                var distanceFromOrigin = DistanceFromOriginTrigger(blockPosition, triggeredBlock.GridPosition);
                triggeredBlock.Triger(distanceFromOrigin);
            }

            // Regenerate
            ScheduleRegenerateBoard();
        }

        private float DistanceFromOriginTrigger(Vector2Int trigger, Vector2Int block)
        {
            return Vector2Int.Distance(trigger, block);
        }


        /**
         * Recursively collect all neighbors of same type to build a full list of blocks in this "chain" in the results list
         */
        private void FindChainRecursive(BlockType type, Vector2Int blockPosition, List<Vector2Int> testedPositions,
            List<Block> results) {
           
            // Check if this block has already been tested
            if (testedPositions.Contains(blockPosition)) {
                return;
            }

            // Check if this block is part of the match
            var block = GetBlockAtPosition(blockPosition);
            if (block == null || block.Type != type) {
                return;
            } 

            // Add this block to the results and mark it as tested
            results.Add(block);
            testedPositions.Add(blockPosition);

            // Recursively search the blocks adjacent to this one
            foreach (var direction in  AdjecentDirections)
            {
                FindChainRecursive(type,blockPosition + direction, testedPositions, results);
            }
        }

        private List<Block> GetSurroundingBlocks(Vector2Int position)
        {
            var surroundingBlocks = new List<Block>();

            foreach (var surroundingDirection in SurroundingDirections)
            {
                surroundingBlocks.Add(GetBlockAtPosition(position +surroundingDirection));
            }
            return surroundingBlocks;
        }
        

        private Block GetBlockAtPosition(Vector2Int blockPosition)
        {
            if (blockPosition.x < 0 || blockPosition.x >= blocks.GetLength(0) || blockPosition.y < 0 || blockPosition.y >= blocks.GetLength(1))
            {
                return null;
            }
            return blocks[blockPosition.x, blockPosition.y];
        }
    }
}