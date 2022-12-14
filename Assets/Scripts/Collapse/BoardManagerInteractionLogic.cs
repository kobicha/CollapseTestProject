using System.Collections.Generic;
using Collapse.Blocks;

namespace Collapse {
    /**
     * Partial class for separating the main functions that are needed to be modified in the context of this test
     */
    public partial class BoardManager {
        
        /**
         * Trigger a bomb
         */
        public void TriggerBomb(Bomb bomb) {
            //TODO: Implement
        }

        /**
         * Trigger a match
         */
        public void TriggerMatch(Block block) {
            // Find all blocks in this match
            var results = new List<Block>();
            var tested = new List<GridPosition>();
            var blockPosition = new GridPosition(block.GridPosition.x, block.GridPosition.y);
            FindChainRecursive(block.Type, blockPosition, tested, results);
            
            // Trigger blocks
            for (var i = 0; i < results.Count; i++) {
                results[i].Triger(i);
            }

            // Regenerate
           // ScheduleRegenerateBoard();
        }


        /**
         * Recursively collect all neighbors of same type to build a full list of blocks in this "chain" in the results list
         */
        private void FindChainRecursive(BlockType type, GridPosition blockPosition, List<GridPosition> testedPositions,
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
            FindChainRecursive(type, blockPosition + new GridPosition(-1,0), testedPositions, results); // up
            FindChainRecursive(type,blockPosition + new GridPosition(1,0), testedPositions, results); // down
            FindChainRecursive(type,blockPosition + new GridPosition(0,-1), testedPositions, results); // left
            FindChainRecursive(type,blockPosition + new GridPosition(0,1), testedPositions, results); // right
        }

        private Block GetBlockAtPosition(GridPosition blockPosition)
        {
            if (blockPosition.x < 0 || blockPosition.x >= blocks.GetLength(1) || blockPosition.y < 0 || blockPosition.y >= blocks.GetLength(0))
            {
                return null;
            }
            return blocks[blockPosition.x, blockPosition.y];
        }
    }
}