namespace WhatsNewInNETLibraryAPIs;

public sealed class DisposableResource
	: IDisposable
{
	private readonly Stream stream = new MemoryStream();
	private bool disposedValue;

	public long GetStreamSize()
	{
#pragma warning disable CA1513 // Use ObjectDisposedException throw helper
		if (this.disposedValue)
		{
			throw new ObjectDisposedException(this.GetType().FullName);
		}
#pragma warning restore CA1513 // Use ObjectDisposedException throw helper

		return this.stream.Length;
	}

	public long GetStreamSizeThrowIf()
	{
		ObjectDisposedException.ThrowIf(this.disposedValue, this);
		return this.stream.Length;
	}

	private void Dispose(bool disposing)
	{
		if (!this.disposedValue)
		{
			if (disposing)
			{
				this.stream.Dispose();
			}

			this.disposedValue = true;
		}
	}

	public void Dispose() => this.Dispose(true);
}